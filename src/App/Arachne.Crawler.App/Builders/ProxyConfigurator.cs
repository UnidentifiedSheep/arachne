using Arachne.Abstractions.Interfaces.Fetcher.Proxy;
using Arachne.Abstractions.Interfaces.HostBuilderConfigurator;
using Fetcher.Proxy;
using Fetcher.Proxy.Rotators;
using Microsoft.Extensions.DependencyInjection;

namespace Arachne.Crawler.App.Builders;

internal class ProxyConfigurator(IServiceCollection services) : IProxyConfigurator
{
    public Type ProxyContainerType { get; private set; } = typeof(ProxyContainer);
    public Type ProxyRotatorType { get; private set; } = typeof(RoundRobinProxyRotator);
    public Type ProxyClientMapper { get; private set; } = typeof(ProxyClientMapper);
    
    public IProxyConfigurator WithProxyContainer<T>() where T : IProxyContainer
    {
        ProxyContainerType = typeof(T);
        return this;
    }

    public IProxyConfigurator WithProxyRotator<T>() where T : IProxyRotator
    {
        ProxyRotatorType = typeof(T);
        return this;
    }

    public IProxyConfigurator WithProxyClientMapper<T>() where T : IProxyClientMapper
    {
        ProxyClientMapper = typeof(T);
        return this;
    }

    public void Build()
    {
        services.AddSingleton(typeof(IProxyContainer), ProxyContainerType);
        services.AddSingleton(typeof(IProxyRotator), ProxyRotatorType);
        services.AddSingleton(typeof(IProxyClientMapper), ProxyClientMapper);
    }
}