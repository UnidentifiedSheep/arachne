using Arachne.Abstractions.Interfaces.Crawler;
using Arachne.Abstractions.Models.Options;

namespace Crawler;

public class RateLimiter : IRateLimiter
{
    private readonly int _windowSizeSeconds = 5;
    private readonly int[] _buckets;
    private readonly long _bucketDurationMs = 1000;
    private int _currentIndex;
    private long _lastTimestamp;

    public int MaxRps { get; private set; }

    public RateLimiter(CrawlerOptions options)
    {
        MaxRps = options.MaxRps;
        _buckets = new int[_windowSizeSeconds];
        _lastTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }

    public void ChangeRps(int newRps)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(newRps);
        MaxRps = newRps;
    }

    public double CurrentRps
    {
        get
        {
            RotateBuckets();
            int sum = 0;
            for (int i = 0; i < _buckets.Length; i++)
                sum += Volatile.Read(ref _buckets[i]);
            return sum / (double)_windowSizeSeconds;
        }
    }

    public async Task WaitTillAllowed()
    {
        while (true)
        {
            RotateBuckets();

            int total = 0;
            for (int i = 0; i < _buckets.Length; i++)
                total += Volatile.Read(ref _buckets[i]);

            if (total < MaxRps * _windowSizeSeconds)
            {
                Interlocked.Increment(ref _buckets[_currentIndex]);
                return;
            }

            long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            long elapsed = now - _lastTimestamp;
            long waitMs = Math.Max(1, _bucketDurationMs - elapsed);
            await Task.Delay((int)waitMs);
        }
    }

    private void RotateBuckets()
    {
        long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        long elapsedBuckets = (now - _lastTimestamp) / _bucketDurationMs;
        if (elapsedBuckets > 0)
        {
            for (long i = 0; i < Math.Min(elapsedBuckets, _buckets.Length); i++)
            {
                int index = (int)((_currentIndex + i + 1) % _buckets.Length);
                Volatile.Write(ref _buckets[index], 0);
            }
            _currentIndex = (int)((_currentIndex + elapsedBuckets) % _buckets.Length);
            _lastTimestamp = now;
        }
    }
}