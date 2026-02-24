using System.Diagnostics;
using Arachne.Abstractions.Interfaces.Crawler;

namespace Crawler.CrawlerMetrics;

public sealed class MetricsTaskTimer(ICrawlerMetrics crawlerMetrics) : IDisposable
{
    private readonly Stopwatch _sw = Stopwatch.StartNew();
    public void Dispose()
    {
        _sw.Stop();
        crawlerMetrics.AddTaskRunTime(_sw.ElapsedMilliseconds);
    }
}