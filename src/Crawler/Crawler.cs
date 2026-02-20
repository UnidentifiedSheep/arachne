using System.Threading.Channels;
using Arachne.Abstractions.EventArgs;
using Arachne.Abstractions.Interfaces.Crawler;
using Arachne.Abstractions.Interfaces.Fetcher.Pipeline;
using Arachne.Abstractions.Models.Fetcher;

namespace Crawler;

public sealed class Crawler(IPipelineExecutor pipelineExecutor, IRateLimiter rateLimiter, IConcurrencyLimiter concurrencyLimiter)
    : ICrawler
{
    private readonly Channel<FetcherContext> _channel = Channel.CreateUnbounded<FetcherContext>();

    private Task? _processingTask;
    private readonly Lock _startLock = new();

    public bool AddCrawlJob(FetcherContext context)
    {
        var written = _channel.Writer.TryWrite(context);
        if (written) EnsureProcessingStarted();
        return written;
    }

    private void EnsureProcessingStarted()
    {
        if (_processingTask is { IsCompleted: false }) return;

        lock (_startLock)
        {
            if (_processingTask is { IsCompleted: false }) return;
            _processingTask = Task.Run(ProcessQueueAsync);
        }
    }

    private async Task ProcessQueueAsync()
    {
        await foreach (var context in _channel.Reader.ReadAllAsync())
        {
            using var concur = await concurrencyLimiter.WaitAsync();
            await rateLimiter.WaitTillAllowed();

            _ = ProcessSingleAsync(context).ContinueWith(ContinueOnException);
        }
    }

    private async Task ProcessSingleAsync(FetcherContext context)
    {
        var result = await pipelineExecutor.ExecutePipeline(context);
        OnFetchCompleted?.Invoke(this, new FetcherResultEventArgs(result));
    }

    private void ContinueOnException(Task t)
    {
        if (t.Exception == null) return;
        OnFetchFaulted?.Invoke(this, new FetcherFaultEventArgs(t.Exception));
    }

    public Task CrawlAsync(CancellationToken cancellationToken = default) => _processingTask ?? Task.CompletedTask;

    public event EventHandler<FetcherResultEventArgs>? OnFetchCompleted;
    public event EventHandler<FetcherFaultEventArgs>? OnFetchFaulted;
}