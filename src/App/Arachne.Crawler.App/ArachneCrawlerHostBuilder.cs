using Arachne.Abstractions.Interfaces.HostBuilderConfigurator;
using Arachne.Crawler.App.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace Arachne.Crawler.App;

public sealed class ArachneCrawlerHostBuilder
{
    public IServiceCollection Services { get; } = new ServiceCollection();

    private bool _built;
    
    private readonly ProxyConfigurator _proxyConfigurator;
    private readonly UserAgentConfigurator _agentConfigurator;
    private readonly FetcherConfigurator _fetcherConfigurator;
    private readonly CrawlerConfigurator _crawlerConfigurator;
    private readonly CrawlerOptionsConfigurator _crawlerOptionsConfigurator;
    
    public ArachneCrawlerHostBuilder()
    {
        _proxyConfigurator = new ProxyConfigurator(Services);
        _agentConfigurator = new UserAgentConfigurator(Services);
        _fetcherConfigurator = new FetcherConfigurator(Services);
        _crawlerConfigurator = new CrawlerConfigurator(Services);
        _crawlerOptionsConfigurator = new CrawlerOptionsConfigurator(Services);
    }

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

    public ArachneCrawlerApp Build()
    {
        if (_built) throw new InvalidOperationException("HostBuilder can only be built once.");
        
        _built = true;
        _proxyConfigurator.Build();
        _agentConfigurator.Build();
        _crawlerConfigurator.Build();
        _crawlerOptionsConfigurator.Build();
        _fetcherConfigurator.Build();
        
        return new ArachneCrawlerApp(Services);
    }
}