using Arachne.Abstractions.Interfaces.Crawler;

namespace Crawler.ConcurrencyLimiter;

public class ConcurrencyLimiter : IConcurrencyLimiter
{
    private readonly SemaphoreSlim _semaphore;
    public int MaxConcurrency { get; }
    public int CurrentCount => _semaphore.CurrentCount;

    public ConcurrencyLimiter(int maxConcurrency = 1)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(maxConcurrency, 1);
        MaxConcurrency = maxConcurrency;
        _semaphore = new SemaphoreSlim(maxConcurrency);
    }

    public async Task<IDisposable> WaitAsync(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        return new SemaphoreReleaser(_semaphore);
    }
}