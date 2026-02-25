using Arachne.Abstractions.Interfaces.Fetcher;
using Arachne.Abstractions.Models.Fetcher;

namespace Fetcher.FetcherMiddlewares;

public class SendMiddleware : IFetcherMiddleware
{
    private static readonly HttpClient HttpClient = new();
    public async Task<FetcherResult> InvokeAsync(FetcherContext context, CancellationToken token, FetcherDelegate next)
    {
        var client = context.HttpClient ?? HttpClient;
        using HttpRequestMessage request = context.CreateRequestMessage();
        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, token);
        var content = await response.Content.ReadAsStringAsync(token);

        return new FetcherResult(content, response.StatusCode, context);
    }
}