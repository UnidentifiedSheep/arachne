using System.Net;

namespace Arachne.Abstractions.Models.Fetcher;

public record FetcherResult(string? Result, HttpStatusCode StatusCode, FetcherContext Context);