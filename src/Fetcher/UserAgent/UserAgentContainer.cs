using System.Collections.Immutable;
using Arachne.Abstractions.Interfaces.Fetcher.UserAgent;
using Fetcher.Constants;

namespace Fetcher.UserAgent;

public class UserAgentContainer : IUserAgentContainer
{
    private readonly List<string> _userAgents;
    private ImmutableList<string> _immutableUserAgents;
    public IImmutableList<string> UserAgents => _immutableUserAgents;
    
    /// <summary>
    /// User agent container.
    /// </summary>
    /// <param name="userAgents">If null inits with UserAgentConstants.UserAgents as default.</param>
    public UserAgentContainer(IEnumerable<string>? userAgents = null)
    {
        userAgents ??= UserAgentConstants.UserAgents;
        _userAgents = userAgents.Distinct().ToList();
        _immutableUserAgents = _userAgents.ToImmutableList();
    }
    
    public void Add(string userAgent)
    {
        if (_userAgents.Contains(userAgent)) return;
        _userAgents.Add(userAgent);
        _immutableUserAgents = _userAgents.ToImmutableList();
    }

    public void Remove(string userAgent)
    {
        if (_userAgents.Remove(userAgent))
            _immutableUserAgents = _userAgents.ToImmutableList();
    }

    public void Clear()
    {
        _userAgents.Clear();
        _immutableUserAgents = _userAgents.ToImmutableList();
    }
}