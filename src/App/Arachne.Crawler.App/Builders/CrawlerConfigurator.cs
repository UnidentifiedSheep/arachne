using Arachne.Abstractions.Interfaces.Crawler;
using Arachne.Abstractions.Interfaces.HostBuilderConfigurator;
using Arachne.Abstractions.Interfaces.HostBuilders;
using Crawler;
using Crawler.ConcurrencyLimiter;
using Microsoft.Extensions.DependencyInjection;

namespace Arachne.Crawler.App.Builders;

internal class CrawlerConfigurator : ICrawlerConfigurator, IConfigurator
{
    public Type RateLimiterType = typeof(RateLimiter);
    public Type ConcurrencyLimiterType = typeof(ConcurrencyLimiter);
    public Type CrawlerType = typeof(global::Crawler.Crawler);
    
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

    public void Build(IAppHostBuilder builder)
    {
        builder.Services.AddSingleton(typeof(IRateLimiter), RateLimiterType);
        builder.Services.AddSingleton(typeof(IConcurrencyLimiter), ConcurrencyLimiterType);
        builder.Services.AddSingleton(typeof(ICrawler), CrawlerType);
    }
}