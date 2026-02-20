using Arachne.Abstractions.Models.Fetcher;

namespace Arachne.Abstractions.Interfaces.Fetcher;

public delegate Task<FetcherResult> FetcherDelegate(FetcherContext context, CancellationToken token);

public interface IFetcherMiddleware
{
    Task<FetcherResult> InvokeAsync(FetcherContext context, CancellationToken token, FetcherDelegate next);
}