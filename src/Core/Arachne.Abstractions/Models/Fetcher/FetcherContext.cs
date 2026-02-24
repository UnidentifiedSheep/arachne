using System.Net;
using System.Text;
using System.Web;

namespace Arachne.Abstractions.Models.Fetcher;

public sealed class FetcherContext
{
    public readonly Guid Id = Guid.NewGuid();
    public string Url { get; }
    public HttpMethod Method { get; }
    public int RetryCount { get; }
    public HashSet<HttpStatusCode> RetryOn { get; }
    public int DelayMs { get; }
    public double DelayMultiplier { get; }

    private readonly Dictionary<string, string> _headers = new();
    private readonly Dictionary<string, string> _queryParameters;
    
    public IReadOnlyDictionary<string, string> Headers => _headers;
    public IReadOnlyDictionary<string, string> QueryParameters => _queryParameters;

    private readonly HashSet<string> _processorTags = [];
    public IReadOnlyCollection<string> ProcessorTags => _processorTags;
    
    public HttpContent? Content { get; private set; }
    public HttpClient? HttpClient { get; private set; }


    public FetcherContext(string url, HttpMethod method, int retryCount = 0, int delayMs = 0, double delayMultiplier = 1, 
        params HttpStatusCode[] retryOn)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(retryCount);
        ArgumentOutOfRangeException.ThrowIfNegative(delayMs);
        ArgumentOutOfRangeException.ThrowIfLessThan(delayMultiplier, 1);

        Url = url ?? throw new ArgumentNullException(nameof(url));
        Method = method ?? throw new ArgumentNullException(nameof(method));
        RetryCount = retryCount;
        DelayMs = delayMs;
        DelayMultiplier = delayMultiplier;
        RetryOn = retryOn.ToHashSet();

        (Url, _queryParameters) = ParseQueryParametersFromUrl(url);
    }
    
    private (string path, Dictionary<string, string> query) ParseQueryParametersFromUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) return (url, new Dictionary<string, string>());

        var uri = new Uri(url);
        var query = HttpUtility.ParseQueryString(uri.Query);
        var dict = new Dictionary<string, string>();
        foreach (string? key in query.AllKeys)
        {
            if (key != null)
                dict[key] = query[key]!;
        }

        var path = uri.GetLeftPart(UriPartial.Path);
        return (path, dict);
    }
    
    public FetcherContext(Guid id, string url, HttpMethod method, int retryCount = 0, int delayMs = 0, double delayMultiplier = 1, 
        params HttpStatusCode[] retryOn) : this(url, method, retryCount, delayMs, delayMultiplier, retryOn)
    {
        Id = id;
    }

    public FetcherContext WithHeader(string key, string value)
    {
        _headers[key] = value;
        return this;
    }

    public FetcherContext WithQuery(string key, string value)
    {
        _queryParameters[key] = value;
        return this;
    }

    /// <summary>
    /// Adds a processor tag.
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public FetcherContext WithProcessorTag(string tag)
    {
        _processorTags.Add(tag);
        return this;
    }

    /// <summary>
    /// Clears processor tags, and adds new ones.
    /// </summary>
    /// <param name="tags"></param>
    /// <returns></returns>
    public FetcherContext WithProcessorTags(IEnumerable<string> tags)
    {
        _processorTags.Clear();
        _processorTags.UnionWith(tags);
        return this;
    }

    public FetcherContext WithJsonContent(string json)
    {
        Content = new StringContent(json, Encoding.UTF8, "application/json");
        return this;
    }

    public FetcherContext WithContent(HttpContent content)
    {
        Content = content;
        return this;
    }

    public FetcherContext WithHttpClient(HttpClient client)
    {
        HttpClient = client;
        return this;
    }

    public HttpRequestMessage CreateRequestMessage()
    {
        var finalUrl = BuildUrlWithQuery();
        var request = new HttpRequestMessage(Method, finalUrl);

        foreach (var header in _headers)
            request.Headers.TryAddWithoutValidation(header.Key, header.Value);

        request.Content = Content;

        return request;
    }

    private string BuildUrlWithQuery()
    {
        if (_queryParameters.Count == 0)
            return Url;

        var uriBuilder = new UriBuilder(Url);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        foreach (var kv in _queryParameters)
            query[kv.Key] = kv.Value;

        uriBuilder.Query = query.ToString()!;
        return uriBuilder.ToString();
    }
}