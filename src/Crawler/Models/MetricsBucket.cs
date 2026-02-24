namespace Crawler.Models;

public sealed class MetricsBucket
{
    public long TotalRunTime;
    public long Count;
    public long MaxRunTime;
    public long MinRunTime = long.MaxValue;

    public void Reset()
    {
        TotalRunTime = 0;
        Count = 0;
        MaxRunTime = 0;
        MinRunTime = long.MaxValue;
    }
}