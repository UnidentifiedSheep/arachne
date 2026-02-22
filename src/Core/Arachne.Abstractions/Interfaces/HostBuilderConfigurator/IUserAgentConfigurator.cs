using Arachne.Abstractions.Interfaces.Fetcher.UserAgent;

namespace Arachne.Abstractions.Interfaces.HostBuilderConfigurator;

public interface IUserAgentConfigurator
{
    IUserAgentConfigurator WithUserAgentRotator<T>() where T : IUserAgentRotator;
    IUserAgentConfigurator WithUserAgentContainer<T>() where T : IUserAgentContainer;
}