using System.Collections.Immutable;

namespace Arachne.Abstractions.Interfaces.Fetcher.UserAgent;

public interface IUserAgentContainer
{
    IImmutableList<string> UserAgents { get; }
    void Add(string userAgent);
    void Remove(string userAgent);
    void Clear();
}