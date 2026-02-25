using Arachne.Abstractions.Interfaces.Crawler;
using Arachne.Abstractions.Interfaces.Fetcher;

namespace Arachne.Abstractions.Interfaces.HostBuilderConfigurator;

public interface ICrawlerConfigurator
{
    ICrawlerConfigurator WithRateLimiter<T>() where T : IRateLimiter;
    ICrawlerConfigurator WithCrawler<T>() where T : ICrawler;
    ICrawlerConfigurator WithCrawlerMetrics<T>() where T : ICrawlerMetrics;
    ICrawlerConfigurator WithMaxRps(int maxRps);
    ICrawlerConfigurator WithWorkerCount(int workerCount);
}