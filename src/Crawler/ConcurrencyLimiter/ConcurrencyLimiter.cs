using Arachne.Abstractions.Interfaces.Crawler;
using Arachne.Abstractions.Models.Options;

namespace Crawler.ConcurrencyLimiter;

public class ConcurrencyLimiter(CrawlerOptions options) : IConcurrencyLimiter
{
    private readonly SemaphoreSlim _semaphore = new(options.MaxConcurrency);
    public int MaxConcurrency { get; } = options.MaxConcurrency;
    public int CurrentCount => _semaphore.CurrentCount;

    public async Task<IDisposable> WaitAsync(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        return new SemaphoreReleaser(_semaphore);
    }
}