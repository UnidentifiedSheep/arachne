using Arachne.Abstractions.Interfaces.Fetcher;
using Arachne.Abstractions.Interfaces.Fetcher.UserAgent;
using Arachne.Abstractions.Models.Fetcher;

namespace Fetcher.FetcherMiddlewares;

public class UserAgentMiddleware(IUserAgentRotator rotator) : IFetcherMiddleware
{
    public async Task<FetcherResult> InvokeAsync(FetcherContext context, CancellationToken token, FetcherDelegate next)
    {
        context.WithHeader("User-Agent", rotator.GetNext() ?? string.Empty);
        return await next(context, token);
    }
}