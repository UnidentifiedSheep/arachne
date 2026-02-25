using Arachne.Abstractions.Interfaces.Crawler;
using Arachne.Abstractions.Interfaces.HostBuilderConfigurator;
using Arachne.Abstractions.Interfaces.HostBuilders;
using Arachne.Abstractions.Models.Options;
using Crawler;
using Crawler.BackgroundServices;
using Crawler.CrawlerMetrics;
using Microsoft.Extensions.DependencyInjection;

namespace Arachne.Crawler.App.Builders;

internal class CrawlerConfigurator : ICrawlerConfigurator, IConfigurator
{
    public Type RateLimiterType = typeof(RateLimiter);
    public Type CrawlerType = typeof(global::Crawler.Crawler);
    public Type CrawlerMetricsType = typeof(CrawlerMetrics);
    public int MaxRps { get; private set; } = int.MaxValue;
    public int WorkerCount { get; private set; } = 10;
    public ICrawlerConfigurator WithRateLimiter<T>() where T : IRateLimiter
    {
        RateLimiterType = typeof(T);
        return this;
    }

    public ICrawlerConfigurator WithCrawler<T>() where T : ICrawler
    {
        CrawlerType = typeof(T);
        return this;
    }
    
    public ICrawlerConfigurator WithCrawlerMetrics<T>() where T : ICrawlerMetrics
    {
        CrawlerMetricsType = typeof(T);
        return this;
    }

    public ICrawlerConfigurator WithMaxRps(int maxRps)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxRps);
        MaxRps = maxRps;
        return this;
    }

    public ICrawlerConfigurator WithWorkerCount(int workerCount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(workerCount);
        WorkerCount = workerCount;
        return this;
    }

    public void Build(IAppHostBuilder builder)
    {
        builder.Services.AddSingleton(typeof(IRateLimiter), RateLimiterType);
        builder.Services.AddSingleton(typeof(ICrawler), CrawlerType);
        builder.Services.AddSingleton(typeof(ICrawlerMetrics), CrawlerMetricsType);
        builder.Services.AddSingleton<CrawlerOptions>(_ => new CrawlerOptions
        {
            MaxRps = MaxRps,
            WorkerCount = WorkerCount
        });
        builder.Services.AddHostedService<CrawlWorker>();
        
    }
}