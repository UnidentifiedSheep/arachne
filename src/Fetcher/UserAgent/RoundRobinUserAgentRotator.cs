using Arachne.Abstractions.Interfaces.Fetcher.UserAgent;

namespace Fetcher.UserAgent;

public class RoundRobinUserAgentRotator(IUserAgentContainer container) : IUserAgentRotator
{
    private volatile int _currentIndex = -1;

    public string? GetNext()
    {
        if (container.UserAgents.Count == 0) return null;
        int nextIndex = (_currentIndex + 1) % container.UserAgents.Count;
        _currentIndex = nextIndex;
        return container.UserAgents[nextIndex];
    }
}