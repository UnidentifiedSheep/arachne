using Arachne.Abstractions.Interfaces.Fetcher.Proxy;

namespace Fetcher.Proxy.Rotators;

public class RoundRobinProxyRotator(IProxyContainer proxyContainer) : IProxyRotator
{
    private volatile int _currentIndex = -1;
    
    public IProxy? GetNext()
    {
        if (proxyContainer.Proxies.Count == 0) return null;
        int nextIndex = (_currentIndex + 1) % proxyContainer.Proxies.Count;
        _currentIndex = nextIndex;
        return proxyContainer.Proxies[nextIndex];
    }
}