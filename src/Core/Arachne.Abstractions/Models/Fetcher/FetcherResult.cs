using System.Net;

namespace Arachne.Abstractions.Models.Fetcher;

public class FetcherResult
{
    public string? Result { get; init; }
    public HttpStatusCode StatusCode { get; init; }
    public FetcherContext Context { get; init; }
    
    public FetcherResult(string? result, HttpStatusCode statusCode, FetcherContext context)
    {
        Result = result;
        StatusCode = statusCode;
        Context = context;
    }

    public FetcherResult() { }
}