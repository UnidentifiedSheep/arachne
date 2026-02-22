using Arachne.Abstractions.Interfaces.Fetcher.UserAgent;
using Arachne.Abstractions.Interfaces.HostBuilderConfigurator;
using Fetcher.UserAgent;
using Microsoft.Extensions.DependencyInjection;

namespace Arachne.Crawler.App.Builders;

internal class UserAgentConfigurator(IServiceCollection services) : IUserAgentConfigurator
{
    public Type UserAgentContainerType { get; private set; } = typeof(UserAgentContainer);
    public Type UserAgentRotatorType { get; private set; } = typeof(RoundRobinUserAgentRotator);
    
    public IUserAgentConfigurator WithUserAgentRotator<T>() where T : IUserAgentRotator
    {
        UserAgentRotatorType = typeof(T);
        return this;
    }

    public IUserAgentConfigurator WithUserAgentContainer<T>() where T : IUserAgentContainer
    {
        UserAgentContainerType = typeof(T);
        return this;
    }
    
    public void Build()
    {
        services.AddSingleton(typeof(IUserAgentContainer), UserAgentContainerType);
        services.AddSingleton(typeof(IUserAgentRotator), UserAgentRotatorType);
    }
}