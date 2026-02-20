using Arachne.Abstractions.Interfaces.HostBuilder;
using Arachne.Builder.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace Arachne.Builder.HostBuilders;

public sealed class ArachneHostBuilder
{
    private readonly IServiceCollection _services = new ServiceCollection();
    public IServiceCollection Services => _services;
    
    private bool _built;
    
    private readonly ProxyConfigurator _proxyConfigurator;
    private readonly UserAgentConfigurator _agentConfigurator;
    private readonly FetcherConfigurator _fetcherConfigurator;
    private readonly CrawlerConfigurator _crawlerConfigurator;
    private readonly CrawlerOptionsConfigurator _crawlerOptionsConfigurator;
    
    public ArachneHostBuilder()
    {
        _proxyConfigurator = new ProxyConfigurator(_services);
        _agentConfigurator = new UserAgentConfigurator(_services);
        _fetcherConfigurator = new FetcherConfigurator(_services);
        _crawlerConfigurator = new CrawlerConfigurator(_services);
        _crawlerOptionsConfigurator = new CrawlerOptionsConfigurator(_services);
    }

    public ArachneHostBuilder ConfigureFetcher(Action<IFetcherConfigurator> configureFetcher)
    {
        configureFetcher(_fetcherConfigurator);
        return this;
    }

    public ArachneHostBuilder ConfigureProxy(Action<IProxyConfigurator> proxyConfig)
    {
        proxyConfig(_proxyConfigurator);
        return this;
    }
    
    public ArachneHostBuilder ConfigureUserAgent(Action<IUserAgentConfigurator> agentConfig)
    {
        agentConfig(_agentConfigurator);
        return this;
    }

    public ArachneHostBuilder ConfigureCrawler(Action<ICrawlerConfigurator> crawlerConfig)
    {
        crawlerConfig(_crawlerConfigurator);
        return this;
    }
    
    public ArachneHostBuilder ConfigureCrawlerOptions(Action<ICrawlerOptionsConfigurator> crawlerOptionsConfig)
    {
        crawlerOptionsConfig(_crawlerOptionsConfigurator);
        return this;
    }

    public ArachneApp Build()
    {
        if (_built) throw new InvalidOperationException("HostBuilder can only be built once.");
        
        _built = true;
        _proxyConfigurator.Build();
        _agentConfigurator.Build();
        _crawlerConfigurator.Build();
        _crawlerOptionsConfigurator.Build();
        _fetcherConfigurator.Build();
        
        return new ArachneApp(_services);
    }
}