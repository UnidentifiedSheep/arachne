using Arachne.Abstractions.Interfaces.Fetcher.Proxy;

namespace Arachne.Abstractions.Interfaces.HostBuilder;

public interface IProxyConfigurator
{
    IProxyConfigurator WithProxyContainer<T>() where T : IProxyContainer;
    IProxyConfigurator WithProxyRotator<T>() where T : IProxyRotator;
    IProxyConfigurator WithProxyClientMapper<T>() where T : IProxyClientMapper;
}