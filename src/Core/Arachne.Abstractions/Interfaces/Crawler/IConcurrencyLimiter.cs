namespace Arachne.Abstractions.Interfaces.Crawler;

public interface IConcurrencyLimiter
{
    /// <summary>
    /// Max allowed concurrency slots.
    /// </summary>
    int MaxConcurrency { get; }
    /// <summary>
    /// Remaining concurrency slots.
    /// </summary>
    int CurrentCount { get; }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<IDisposable> WaitAsync(CancellationToken cancellationToken = default);
}