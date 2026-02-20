using Arachne.Abstractions.Interfaces.Fetcher;
using Arachne.Abstractions.Interfaces.Fetcher.Pipeline;
using Arachne.Abstractions.Models.Fetcher;

namespace Fetcher.Pipeline;

public class FetcherPipeline : IFetcherPipeline
{
    private readonly List<IFetcherMiddleware> _middlewares = [];

    public IFetcherPipeline AddMiddleware(IFetcherMiddleware middleware)
    {
        _middlewares.Add(middleware);
        return this;
    }

    public Task<FetcherResult> ExecuteAsync(FetcherContext context, CancellationToken token = default)
    {
        FetcherDelegate next = (_, _) => throw new InvalidOperationException("Pipeline reached end and middleware must not call next.");

        foreach (var middleware in _middlewares.AsEnumerable().Reverse())
        {
            var local = next;
            next = (ctx, ct) => middleware.InvokeAsync(ctx, ct, local);
        }

        return next(context, token);
    }
}