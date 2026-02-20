using Arachne.Abstractions.Models.Fetcher;

namespace Arachne.Abstractions.Interfaces.Fetcher.Pipeline;

public interface IFetcherPipeline
{
    IFetcherPipeline AddMiddleware(IFetcherMiddleware middleware);
    Task<FetcherResult> ExecuteAsync(FetcherContext context, CancellationToken token = default);
}