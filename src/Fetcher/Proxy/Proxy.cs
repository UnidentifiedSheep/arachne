using System.Net;
using Arachne.Abstractions.Interfaces.Fetcher.Proxy;

namespace Fetcher.Proxy;

public class Proxy(string host, int port, ICredentials? credentials) : IProxy
{
    public ICredentials? Credentials { get; } = credentials;
    public string Host { get; } = host;
    public int Port { get; } = port;
}