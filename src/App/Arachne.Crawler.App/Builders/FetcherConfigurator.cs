using Arachne.Abstractions.Interfaces.Fetcher;
using Arachne.Abstractions.Interfaces.Fetcher.Pipeline;
using Arachne.Abstractions.Interfaces.HostBuilderConfigurator;
using Fetcher.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace Arachne.Crawler.App.Builders;

internal class FetcherConfigurator(IServiceCollection services) : IFetcherConfigurator
{
    public IFetcherConfigurator UseMiddleware<T>(ServiceLifetime lifetime = ServiceLifetime.Transient) where T : IFetcherMiddleware
    {
        services.Add(new ServiceDescriptor(typeof(IFetcherMiddleware), typeof(T), lifetime));
        return this;
    }
    
    public void Build()
    {
        services.AddScoped<IPipelineExecutor, PipelineExecutor>();
    }
}