using Arachne.Abstractions.Interfaces.Fetcher;
using Arachne.Abstractions.Interfaces.Fetcher.Pipeline;
using Arachne.Abstractions.Models.Fetcher;
using Microsoft.Extensions.DependencyInjection;

namespace Fetcher.Pipeline;

public class PipelineExecutor(IServiceProvider serviceProvider)
{
    public async Task<FetcherResult> ExecutePipeline(FetcherContext context, CancellationToken cancellationToken = default)
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        var middlewareRegistrator = scope.ServiceProvider.GetRequiredService<IMiddlewareRegistrator>();
        if (!middlewareRegistrator.RegisteredMiddlewares.Any())
            throw new ArgumentException("No middlewares registered in IMiddlewareRegistrator.");

        FetcherPipeline pipeline = new();
        foreach (var middlewareType in middlewareRegistrator.RegisteredMiddlewares)
        {
            if (scope.ServiceProvider.GetService(middlewareType) is not IFetcherMiddleware middleware)
                throw new ArgumentException("Middleware not registered in DI container.");
            pipeline.AddMiddleware(middleware);
        }

        return await pipeline.ExecuteAsync(context, cancellationToken);
    }
}