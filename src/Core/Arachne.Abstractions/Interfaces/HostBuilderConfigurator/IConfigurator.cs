using Arachne.Abstractions.Interfaces.HostBuilders;

namespace Arachne.Abstractions.Interfaces.HostBuilderConfigurator;

public interface IConfigurator
{
    void Build(IAppHostBuilder builder);
}