using Arachne.Abstractions.Interfaces.Fetcher;
using Arachne.Abstractions.Interfaces.Fetcher.Proxy;
using Arachne.Abstractions.Models.Fetcher;

namespace Fetcher.FetcherMiddlewares;

public class ProxyMiddleware(IProxyRotator rotator, IProxyClientMapper mapper) : IFetcherMiddleware
{
    public async Task<FetcherResult> InvokeAsync(FetcherContext context, CancellationToken token, FetcherDelegate next)
    {
        var proxy = rotator.GetNext();
        var client = mapper.GetClient(proxy);
        context.WithHttpClient(client);

        return await next(context, token);
    }
}