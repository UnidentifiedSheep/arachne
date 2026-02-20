using Arachne.Abstractions.Models.Fetcher;

namespace Arachne.Abstractions.Interfaces.Fetcher.Pipeline;

public interface IPipelineExecutor
{
    Task<FetcherResult> ExecutePipeline(FetcherContext context, CancellationToken cancellationToken = default);
}