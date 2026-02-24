using Arachne.Abstractions.Abstractions;
using Arachne.Abstractions.Interfaces.HostBuilderConfigurator;
using Arachne.Crawler.App.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace Arachne.Crawler.App;

public sealed class ArachneCrawlerHostBuilder : AppHostBuilder<ArachneCrawlerApp>
{
    public override IServiceCollection Services { get; }

    public ArachneCrawlerHostBuilder(IServiceCollection services)
    {
        Services = services;
    }
    public ArachneCrawlerHostBuilder() : this(new ServiceCollection()) { }

    private bool _built;
    
    private readonly ProxyConfigurator _proxyConfigurator = new();
    private readonly UserAgentConfigurator _agentConfigurator = new();
    private readonly FetcherConfigurator _fetcherConfigurator = new();
    private readonly CrawlerConfigurator _crawlerConfigurator = new();
    private readonly CrawlerOptionsConfigurator _crawlerOptionsConfigurator = new();

    public ArachneCrawlerHostBuilder ConfigureFetcher(Action<IFetcherConfigurator> configureFetcher)
    {
        configureFetcher(_fetcherConfigurator);
        return this;
    }

    public ArachneCrawlerHostBuilder ConfigureProxy(Action<IProxyConfigurator> proxyConfig)
    {
        proxyConfig(_proxyConfigurator);
        return this;
    }
    
    public ArachneCrawlerHostBuilder ConfigureUserAgent(Action<IUserAgentConfigurator> agentConfig)
    {
        agentConfig(_agentConfigurator);
        return this;
    }

    public ArachneCrawlerHostBuilder ConfigureCrawler(Action<ICrawlerConfigurator> crawlerConfig)
    {
        crawlerConfig(_crawlerConfigurator);
        return this;
    }
    
    public ArachneCrawlerHostBuilder ConfigureCrawlerOptions(Action<ICrawlerOptionsConfigurator> crawlerOptionsConfig)
    {
        crawlerOptionsConfig(_crawlerOptionsConfigurator);
        return this;
    }

    public override ArachneCrawlerApp Build()
    {
        if (_built) throw new InvalidOperationException("HostBuilder can only be built once.");
        
        _built = true;
        _proxyConfigurator.Build(this);
        _agentConfigurator.Build(this);
        _crawlerConfigurator.Build(this);
        _crawlerOptionsConfigurator.Build(this);
        _fetcherConfigurator.Build(this);
        
        return new ArachneCrawlerApp(Services);
    }
}