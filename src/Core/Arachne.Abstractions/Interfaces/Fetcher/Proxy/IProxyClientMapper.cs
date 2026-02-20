namespace Arachne.Abstractions.Interfaces.Fetcher.Proxy;

public interface IProxyClientMapper : IDisposable
{
    /// <summary>
    /// Returns HttpClient for needed proxy. If proxy is null, returns HttpClient without proxy.
    /// </summary>
    /// <param name="proxy"></param>
    /// <returns></returns>
    HttpClient GetClient(IProxy? proxy = null);
    /// <summary>
    /// Removes HttpClient for proxy, and releases resources.
    /// </summary>
    /// <param name="proxy"></param>
    void RemoveClient(IProxy proxy);
}