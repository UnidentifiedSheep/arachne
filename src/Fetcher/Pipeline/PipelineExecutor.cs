using Arachne.Abstractions.Interfaces.Fetcher;
using Arachne.Abstractions.Interfaces.Fetcher.Pipeline;
using Arachne.Abstractions.Models.Fetcher;

namespace Fetcher.Pipeline;

public class PipelineExecutor(IEnumerable<IFetcherMiddleware> middlewares) : IPipelineExecutor
{
    public async Task<FetcherResult> ExecutePipeline(FetcherContext context, CancellationToken cancellationToken = default)
    {
        FetcherPipeline pipeline = new();
        foreach (var middleware in middlewares) pipeline.AddMiddleware(middleware);

        return await pipeline.ExecuteAsync(context, cancellationToken);
    }
}