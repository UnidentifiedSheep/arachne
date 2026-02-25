using Arachne.Abstractions.Interfaces.Fetcher;
using Arachne.Abstractions.Interfaces.Fetcher.Pipeline;
using Arachne.Abstractions.Models.Fetcher;

namespace Fetcher.Pipeline;

public class PipelineExecutor : IPipelineExecutor
{
    private readonly FetcherPipeline _pipeline;
    public PipelineExecutor(IEnumerable<IFetcherMiddleware> middlewares)
    {
        _pipeline = new();
        foreach (var middleware in middlewares) _pipeline.AddMiddleware(middleware);
    }

    public async Task<FetcherResult> ExecutePipeline(FetcherContext context, CancellationToken cancellationToken = default)
    {
        return await _pipeline.ExecuteAsync(context, cancellationToken);
    }
}