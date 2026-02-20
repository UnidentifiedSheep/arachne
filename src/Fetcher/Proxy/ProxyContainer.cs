using System.Collections.Immutable;
using Arachne.Abstractions.Interfaces.Fetcher.Proxy;

namespace Fetcher.Proxy;

public class ProxyContainer : IProxyContainer
{
    private readonly List<IProxy> _proxies;
    private ImmutableList<IProxy> _immutableProxies;
    public IImmutableList<IProxy> Proxies => _immutableProxies;
    
    public ProxyContainer(IEnumerable<IProxy> proxies)
    {
        _proxies = proxies.ToList();
        _immutableProxies = _proxies.ToImmutableList();
    }

    public ProxyContainer()
    {
        _proxies = [];
        _immutableProxies = _proxies.ToImmutableList();
    }
    
    public void AddProxy(IProxy proxy)
    {
        _proxies.Add(proxy);
        _immutableProxies = _proxies.ToImmutableList();
    }

    public void RemoveProxy(IProxy proxy)
    {
        if (_proxies.Remove(proxy))
            _immutableProxies = _proxies.ToImmutableList();
    }

    public void RemoveProxy(int index)
    {
        if (index < 0 || index >= _proxies.Count) throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
        _proxies.RemoveAt(index);
        _immutableProxies = _proxies.ToImmutableList();
    }

    public void Clear()
    {
        _proxies.Clear();
        _immutableProxies = _proxies.ToImmutableList();
    }
}