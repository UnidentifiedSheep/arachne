namespace Arachne.Abstractions.Interfaces.Fetcher.Proxy;

/// <summary>
/// Rotates proxies and returns the next proxy.
/// <remarks>DOESN'T CHECK IF PROXY ALIVE</remarks>
/// </summary>
public interface IProxyRotator
{
    /// <summary>
    /// Returns next proxy
    /// </summary>
    /// <returns>Next proxy if found, else null.</returns>
    IProxy? GetNext();
}