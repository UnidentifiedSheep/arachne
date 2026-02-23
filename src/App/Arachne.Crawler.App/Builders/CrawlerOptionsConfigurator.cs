using Arachne.Abstractions.Interfaces.HostBuilderConfigurator;
using Arachne.Abstractions.Interfaces.HostBuilders;
using Arachne.Abstractions.Models.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Arachne.Crawler.App.Builders;

internal class CrawlerOptionsConfigurator : ICrawlerOptionsConfigurator, IConfigurator
{
    public int MaxRps { get; private set; } = int.MaxValue;
    public int MaxConcurrency { get; private set; } = 1;
    
    public ICrawlerOptionsConfigurator WithMaxRps(int maxRps)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxRps);
        MaxRps = maxRps;
        return this;
    }

    public ICrawlerOptionsConfigurator WithMaxConcurrency(int maxConcurrency)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxConcurrency);
        MaxConcurrency = maxConcurrency;
        return this;
    }

    public void Build(IAppHostBuilder builder)
    {
        builder.Services.AddSingleton<CrawlerOptions>(_ => new CrawlerOptions
        {
            MaxRps = MaxRps,
            MaxConcurrency = MaxConcurrency
        });
    }
}