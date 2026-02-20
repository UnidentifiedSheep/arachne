using System.Net;

namespace Arachne.Abstractions.Interfaces.Fetcher.Proxy;

public interface IProxy
{
    string Host { get; }
    int Port { get; }
    ICredentials? Credentials { get; }
}