using System.Net;

namespace Arachne.Contracts.Models;

public sealed class FetcherResult
{
    public string? Result { get; init; }
    public HttpStatusCode StatusCode { get; init; }
    public FetcherContext Context { get; init; } = null!;
}