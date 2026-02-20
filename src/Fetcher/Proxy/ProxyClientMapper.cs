using System.Collections.Concurrent;
using System.Net;
using Arachne.Abstractions.Interfaces.Fetcher.Proxy;

namespace Fetcher.Proxy;

public sealed class ProxyClientMapper : IProxyClientMapper
{
    private readonly ConcurrentDictionary<IProxy, HttpClient> _clients = new();
    private readonly HttpClient _defaultHttpClient;
    private bool _disposed;

    public ProxyClientMapper(HttpClient? defaultHttpClient = null)
    {
        _defaultHttpClient = defaultHttpClient ?? new HttpClient();
    }

    public HttpClient GetClient(IProxy? proxy = null)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(ProxyClientMapper));
        if (proxy == null) return _defaultHttpClient;
        
        return _clients.GetOrAdd(proxy, p =>
        {
            var handler = new HttpClientHandler
            {
                Proxy = new WebProxy(p.Host, p.Port) { Credentials = p.Credentials },
                UseProxy = true
            };
            return new HttpClient(handler);
        });
    }

    public void RemoveClient(IProxy proxy)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(ProxyClientMapper));
        
        if (_clients.TryRemove(proxy, out var client))
            client.Dispose();
    }

    public void Dispose()
    {
        if (_disposed) return;
        
        _defaultHttpClient.Dispose();
        foreach (var client in _clients.Values) client.Dispose();
        
        _disposed = true;
    }
}