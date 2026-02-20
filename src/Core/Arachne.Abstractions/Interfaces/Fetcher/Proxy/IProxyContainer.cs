using System.Collections.Immutable;

namespace Arachne.Abstractions.Interfaces.Fetcher.Proxy;

public interface IProxyContainer
{
    /// <summary>
    /// All proxies
    /// </summary>
    IImmutableList<IProxy> Proxies { get; }
    /// <summary>
    /// Adds proxy
    /// </summary>
    /// <param name="proxy"></param>
    void AddProxy(IProxy proxy);
    /// <summary>
    /// Removes proxy
    /// </summary>
    /// <param name="proxy"></param>
    void RemoveProxy(IProxy proxy);
    /// <summary>
    /// Removes proxy at index
    /// </summary>
    /// <param name="index">index of proxy</param>
    void RemoveProxy(int index);
    /// <summary>
    /// Clears all proxies
    /// </summary>
    void Clear();
}