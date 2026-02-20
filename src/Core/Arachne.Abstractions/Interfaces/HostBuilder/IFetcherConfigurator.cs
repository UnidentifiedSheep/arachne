using Arachne.Abstractions.Interfaces.Fetcher;
using Microsoft.Extensions.DependencyInjection;

namespace Arachne.Abstractions.Interfaces.HostBuilder;

public interface IFetcherConfigurator
{
    IFetcherConfigurator UseMiddleware<T>(ServiceLifetime lifetime = ServiceLifetime.Transient) where T : IFetcherMiddleware;
}