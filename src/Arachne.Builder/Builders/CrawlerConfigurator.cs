using Arachne.Abstractions.Interfaces.Crawler;
using Arachne.Abstractions.Interfaces.HostBuilder;
using Crawler;
using Crawler.ConcurrencyLimiter;
using Microsoft.Extensions.DependencyInjection;

namespace Arachne.Builder.Builders;

public class CrawlerConfigurator(IServiceCollection services) : ICrawlerConfigurator
{
    public Type RateLimiterType = typeof(RateLimiter);
    public Type ConcurrencyLimiterType = typeof(ConcurrencyLimiter);
    public Type CrawlerType = typeof(Crawler.Crawler);
    
    public ICrawlerConfigurator WithRateLimiter<T>() where T : IRateLimiter
    {
        RateLimiterType = typeof(T);
        return this;
    }

    public ICrawlerConfigurator WithConcurrencyLimiter<T>() where T : IConcurrencyLimiter
    {
        ConcurrencyLimiterType = typeof(T);
        return this;
    }

    public ICrawlerConfigurator WithCrawler<T>() where T : ICrawler
    {
        CrawlerType = typeof(T);
        return this;
    }

    public void Build()
    {
        services.AddSingleton(typeof(IRateLimiter), RateLimiterType);
        services.AddSingleton(typeof(IConcurrencyLimiter), ConcurrencyLimiterType);
        services.AddSingleton(typeof(ICrawler), CrawlerType);
    }
}