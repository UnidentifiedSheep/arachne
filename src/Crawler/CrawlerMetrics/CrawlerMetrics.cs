using Arachne.Abstractions.Interfaces.Crawler;
using Crawler.Models;

namespace Crawler.CrawlerMetrics;

public sealed class CrawlerMetrics : ICrawlerMetrics
{
    private const int WindowSizeSeconds = 60;
    private readonly MetricsBucket[] _buckets = new MetricsBucket[WindowSizeSeconds];
    private int _currentIndex;
    private long _lastTimestampMs;
    private ICrawler _crawler;

    public CrawlerMetrics(ICrawler crawler)
    {
        _crawler = crawler;
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        _lastTimestampMs = now;
        for (int i = 0; i < WindowSizeSeconds; i++)
            _buckets[i] = new MetricsBucket();
    }
    
    private void RotateBuckets()
    {
        long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        long elapsedSeconds = (now - _lastTimestampMs) / 1000;

        if (elapsedSeconds <= 0) return;

        for (long i = 0; i < Math.Min(elapsedSeconds, WindowSizeSeconds); i++)
        {
            _currentIndex = (_currentIndex + 1) % WindowSizeSeconds;
            _buckets[_currentIndex].Reset();
        }

        _lastTimestampMs = now;
    }

    public void AddTaskRunTime(long ms)
    {
        RotateBuckets();
        var bucket = _buckets[_currentIndex];

        Interlocked.Add(ref bucket.TotalRunTime, ms);
        Interlocked.Increment(ref bucket.Count);

        long currentMax;
        do
        {
            currentMax = bucket.MaxRunTime;
            if (ms <= currentMax) break;
        } while (Interlocked.CompareExchange(ref bucket.MaxRunTime, ms, currentMax) != currentMax);

        long currentMin;
        do
        {
            currentMin = bucket.MinRunTime;
            if (ms >= currentMin) break;
        } while (Interlocked.CompareExchange(ref bucket.MinRunTime, ms, currentMin) != currentMin);
    }

    public long AverageRunTimeMs
    {
        get
        {
            RotateBuckets();
            long total = 0, count = 0;
            for (int i = 0; i < WindowSizeSeconds; i++)
            {
                total += Volatile.Read(ref _buckets[i].TotalRunTime);
                count += Volatile.Read(ref _buckets[i].Count);
            }

            return count == 0 ? 0 : total / count;
        }
    }

    public long MaxRunTimeMs
    {
        get
        {
            RotateBuckets();
            long max = 0;
            for (int i = 0; i < WindowSizeSeconds; i++)
                max = Math.Max(max, Volatile.Read(ref _buckets[i].MaxRunTime));
            return max;
        }
    }

    public long MinRunTimeMs
    {
        get
        {
            RotateBuckets();
            long min = long.MaxValue;
            for (int i = 0; i < WindowSizeSeconds; i++)
            {
                long v = Volatile.Read(ref _buckets[i].MinRunTime);
                if (v < min) min = v;
            }
            return min == long.MaxValue ? 0 : min;
        }
    }

    private long _successCount;
    public long SuccessCount => Volatile.Read(ref _successCount);
    public void IncrementSuccess() => Interlocked.Increment(ref _successCount);

    private long _failureCount;
    public long FailureCount => Volatile.Read(ref _failureCount);
    public void IncrementFailure() => Interlocked.Increment(ref _failureCount);
    public int QueueLength => _crawler.Reader.Count;

    public IDisposable MeasureTaskTime() => new MetricsTaskTimer(this);
}