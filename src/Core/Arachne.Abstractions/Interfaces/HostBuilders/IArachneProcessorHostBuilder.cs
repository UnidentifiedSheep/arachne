using Microsoft.Extensions.DependencyInjection;

namespace Arachne.Abstractions.Interfaces.HostBuilders;

public interface IArachneProcessorHostBuilder
{
    IServiceCollection Services { get; }
}