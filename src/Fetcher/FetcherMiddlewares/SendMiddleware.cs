using Arachne.Abstractions.Interfaces.Fetcher;
using Arachne.Abstractions.Models.Fetcher;

namespace Fetcher.FetcherMiddlewares;

public class SendMiddleware(HttpClient defaultClient) : IFetcherMiddleware
{
    public async Task<FetcherResult> InvokeAsync(FetcherContext context, CancellationToken token, FetcherDelegate next)
    {
        var client = context.HttpClient ?? defaultClient;
        using HttpRequestMessage request = context.CreateRequestMessage();
        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, token);
        var content = await response.Content.ReadAsStringAsync(token);

        return new FetcherResult(content, response.StatusCode, context);
    }
}