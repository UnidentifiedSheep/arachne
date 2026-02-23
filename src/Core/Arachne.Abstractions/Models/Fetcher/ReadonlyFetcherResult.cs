using System.Net;

namespace Arachne.Abstractions.Models.Fetcher;

public record ReadonlyFetcherResult(string? Result, HttpStatusCode StatusCode, ReadonlyFetcherContext Context);