using Arachne.Abstractions.Interfaces.Crawler;
using Arachne.Abstractions.Interfaces.Fetcher.Pipeline;
using Arachne.Abstractions.Models.Fetcher;
using Arachne.Contracts.Events;
using Arachne.Extensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Crawler;

public sealed class Crawler(IServiceProvider serviceProvider, IRateLimiter rateLimiter, IConcurrencyLimiter concurrencyLimiter,
    ICrawlerMetrics metrics, ILogger<Crawler> logger) : ICrawler, IAsyncDisposable
{
    private readonly List<Task> _runningTasks = [];
    private readonly Lock _tasksLock = new();
    private bool _isAvailable;
    public (Guid, bool) AddCrawlJob(FetcherContext context)
    {
        if (!_isAvailable) return (Guid.Empty, false);
        if (logger.IsEnabled(LogLevel.Information)) 
            logger.LogInformation("Adding job {id} to queue. Current rps {rps}", context.Id, rateLimiter.CurrentRps);
        var task = ProcessSingleAsync(context, CancellationToken.None);

        lock (_tasksLock)
            _runningTasks.Add(task);

        _ = TrackCompletionAsync(task);

        return (context.Id, true);
    }

    public Task StartAsync(CancellationToken token = default)
    {
        if (logger.IsEnabled(LogLevel.Information)) 
            logger.LogInformation("Starting crawler.");
        _isAvailable = true;
        return Task.CompletedTask;
    }

    public async Task StopAsync()
    {
        _isAvailable = false;
        Task[] remaining;
        lock (_tasksLock) remaining = _runningTasks.ToArray();
        
        await Task.WhenAll(remaining);
    }

    private async Task ProcessSingleAsync(FetcherContext context, CancellationToken token)
    {
        await rateLimiter.WaitTillAllowed().ConfigureAwait(false);
        using var lease = await concurrencyLimiter.WaitAsync(token).ConfigureAwait(false);
        
        // ReSharper disable once InconsistentlySynchronizedField
        metrics.SetQueueLength(_runningTasks.Count);
        
        using var taskTimer = metrics.MeasureTaskTime();
        await using var scope = serviceProvider.CreateAsyncScope();
        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
        var pipelineExecutor = scope.ServiceProvider.GetRequiredService<IPipelineExecutor>();
        
        try
        {
            var result = await pipelineExecutor.ExecutePipeline(context, token).ConfigureAwait(false);
            await publishEndpoint.Publish(new FetchCompletedEvent { Result = result.ToContract() }, token)
                .ConfigureAwait(false);
            metrics.IncrementSuccess();
        }
        catch (Exception ex)
        {
            await publishEndpoint.Publish(new FetchFaultedEvent
            {
                Context = context.ToContract(),
                Exception = ex.Message
            }, token).ConfigureAwait(false);
            metrics.IncrementFailure();
        }
    }

    private async Task TrackCompletionAsync(Task task)
    {
        try
        {
            await task.ConfigureAwait(false);
        }
        finally
        {
            lock (_tasksLock)
                _runningTasks.Remove(task);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await StopAsync();
    }
}