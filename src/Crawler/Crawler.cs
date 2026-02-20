using System.Threading.Channels;
using Arachne.Abstractions.EventArgs;
using Arachne.Abstractions.Interfaces.Crawler;
using Arachne.Abstractions.Interfaces.Fetcher.Pipeline;
using Arachne.Abstractions.Models.Fetcher;
using Microsoft.Extensions.DependencyInjection;

namespace Crawler;

public sealed class Crawler(IServiceProvider serviceProvider, IRateLimiter rateLimiter, IConcurrencyLimiter concurrencyLimiter)
    : ICrawler, IAsyncDisposable
{
    private readonly Channel<FetcherContext> _channel =
        Channel.CreateUnbounded<FetcherContext>();

    private readonly List<Task> _runningTasks = [];
    private readonly Lock _tasksLock = new();

    private CancellationTokenSource? _cts;
    private Task? _processingTask;

    public bool AddCrawlJob(FetcherContext context)
    {
        return _channel.Writer.TryWrite(context);
    }

    public Task StartAsync(CancellationToken token = default)
    {
        if (_processingTask != null)
            return _processingTask;

        _cts = CancellationTokenSource.CreateLinkedTokenSource(token);
        _processingTask = Task.Run(() => ProcessQueueAsync(_cts.Token), token);
        return Task.CompletedTask;
    }

    public async Task StopAsync()
    {
        _channel.Writer.Complete();

        if (_processingTask != null) await _processingTask;

        Task[] remaining;
        lock (_tasksLock) remaining = _runningTasks.ToArray();

        await Task.WhenAll(remaining);
    }

    private async Task ProcessQueueAsync(CancellationToken token)
    {
        await foreach (var context in _channel.Reader.ReadAllAsync(token))
        {
            await rateLimiter.WaitTillAllowed();

            var task = ProcessSingleAsync(context, token);

            lock (_tasksLock) _runningTasks.Add(task);

            _ = TrackCompletionAsync(task);
        }
    }

    private async Task ProcessSingleAsync(FetcherContext context, CancellationToken token)
    {
        using var lease = await concurrencyLimiter.WaitAsync(token);
        await using var scope = serviceProvider.CreateAsyncScope();
        var pipelineExecutor = scope.ServiceProvider.GetRequiredService<IPipelineExecutor>();
        
        try
        {
            var result = await pipelineExecutor.ExecutePipeline(context, token);
            OnFetchCompleted?.Invoke(this,
                new FetcherResultEventArgs(result));
        }
        catch (Exception ex)
        {
            OnFetchFaulted?.Invoke(this, new FetcherFaultEventArgs(ex));
        }
    }

    private async Task TrackCompletionAsync(Task task)
    {
        try
        {
            await task;
        }
        finally
        {
            lock (_tasksLock)
                _runningTasks.Remove(task);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_cts != null) await _cts.CancelAsync();

        await StopAsync();
        _cts?.Dispose();
    }


    public event EventHandler<FetcherResultEventArgs>? OnFetchCompleted;
    public event EventHandler<FetcherFaultEventArgs>? OnFetchFaulted;
}