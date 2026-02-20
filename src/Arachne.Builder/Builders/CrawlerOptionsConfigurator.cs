using Arachne.Abstractions.Interfaces.HostBuilder;
using Arachne.Abstractions.Models.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Arachne.Builder.Builders;

public class CrawlerOptionsConfigurator(IServiceCollection services) : ICrawlerOptionsConfigurator
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

    public void Build()
    {
        services.AddSingleton<CrawlerOptions>(_ => new CrawlerOptions
        {
            MaxRps = MaxRps,
            MaxConcurrency = MaxConcurrency
        });
    }
}