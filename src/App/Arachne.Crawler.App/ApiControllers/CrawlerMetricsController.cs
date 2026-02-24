using Arachne.Abstractions.Interfaces.Crawler;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;

namespace Arachne.Crawler.App.ApiControllers;

public class GetCrawlerMetricsResult
{
    public long AverageRunTimeMs { get; init; }
    public long MaxRunTimeMs { get; init; }
    public long MinRunTimeMs { get; init; }
    public long SuccessCount { get; init; }
    public long FailureCount { get; init; }
    public int QueueLength { get; init; }
    public double Rps { get; init; }
}

public class CrawlerMetricsController(ICrawlerMetrics crawlerMetrics, IRateLimiter rateLimiter) : WebApiController
{
    [Route(HttpVerbs.Get, "/metrics")]
    public GetCrawlerMetricsResult GetMetrics()
    {
        return new GetCrawlerMetricsResult
        {
            AverageRunTimeMs = crawlerMetrics.AverageRunTimeMs,
            MaxRunTimeMs = crawlerMetrics.MaxRunTimeMs,
            MinRunTimeMs = crawlerMetrics.MinRunTimeMs,
            SuccessCount = crawlerMetrics.SuccessCount,
            FailureCount = crawlerMetrics.FailureCount,
            QueueLength = crawlerMetrics.QueueLength,
            Rps = rateLimiter.CurrentRps
        };
    }
}