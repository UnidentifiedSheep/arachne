using System.Net;

namespace Arachne.Abstractions.Models.Fetcher;

public sealed class ReadonlyFetcherContext
{
    public Guid Id { get; }
    public string Url { get; }
    public HttpMethod Method { get; }
    public int RetryCount { get; }
    public int DelayMs { get; }
    public double DelayMultiplier { get; }
    public IReadOnlyCollection<HttpStatusCode> RetryOn { get; }
    public IReadOnlyDictionary<string, string> Headers { get; }
    public IReadOnlyDictionary<string, string> QueryParameters { get; }
    public IReadOnlyCollection<string> ProcessorTags { get; }
    
    public ReadonlyFetcherContext(Guid id, string url, HttpMethod method, int retryCount, int delayMs, double delayMultiplier, 
        IEnumerable<HttpStatusCode> retryOn, Dictionary<string, string> headers, 
        Dictionary<string, string> queryParameters, IEnumerable<string> processorTags)
    {
        Id = id;
        Url = url;
        Method = method;
        RetryCount = retryCount;
        DelayMs = delayMs;
        DelayMultiplier = delayMultiplier;
        RetryOn = retryOn.ToList();
        Headers = headers;
        QueryParameters = queryParameters;
        ProcessorTags = processorTags.ToList();
    }
}
