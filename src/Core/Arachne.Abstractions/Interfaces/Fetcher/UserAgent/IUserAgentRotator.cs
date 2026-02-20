namespace Arachne.Abstractions.Interfaces.Fetcher.UserAgent;

public interface IUserAgentRotator
{
    string? GetNext();
}