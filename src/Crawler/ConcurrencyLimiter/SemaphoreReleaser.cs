namespace Crawler.ConcurrencyLimiter;

internal sealed class SemaphoreReleaser(SemaphoreSlim semaphore) : IDisposable
{
    public void Dispose() => semaphore.Release();
}