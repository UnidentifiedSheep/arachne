using Arachne.Abstractions.Interfaces.Crawler;

namespace Arachne.Abstractions.Interfaces.HostBuilder;

public interface ICrawlerConfigurator
{
    ICrawlerConfigurator WithRateLimiter<T>() where T : IRateLimiter;
    ICrawlerConfigurator WithConcurrencyLimiter<T>() where T : IConcurrencyLimiter;
    ICrawlerConfigurator WithCrawler<T>() where T : ICrawler;
}