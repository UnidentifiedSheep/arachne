using Arachne.Abstractions.Interfaces.Fetcher;
using Arachne.Abstractions.Interfaces.Fetcher.Pipeline;
using Arachne.Abstractions.Interfaces.HostBuilderConfigurator;
using Arachne.Abstractions.Interfaces.HostBuilders;
using Fetcher.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace Arachne.Crawler.App.Builders;

internal class FetcherConfigurator : IFetcherConfigurator, IConfigurator
{
    private readonly List<(Type, ServiceLifetime)> _middlewares = [];
    public IFetcherConfigurator UseMiddleware<T>(ServiceLifetime lifetime = ServiceLifetime.Transient) where T : IFetcherMiddleware
    {
        _middlewares.Add((typeof(T), lifetime));
        return this;
    }

    public void Build(IAppHostBuilder builder)
    {
        builder.Services.AddScoped<IPipelineExecutor, PipelineExecutor>();
        foreach (var middleware in _middlewares)
            builder.Services.Add(new ServiceDescriptor(typeof(IFetcherMiddleware), middleware.Item1, middleware.Item2));
    }
}