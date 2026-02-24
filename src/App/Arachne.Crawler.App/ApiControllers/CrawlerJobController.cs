using System.Net;
using Arachne.Abstractions.Interfaces.Crawler;
using Arachne.Abstractions.Models.Fetcher;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;

namespace Arachne.Crawler.App.ApiControllers;

public class AddCrawlerJobResult
{
    public bool Success { get; init; }
};

public record AddCrawlerJobRequest
{
    public required string Url { get; init; }
    public required string Method { get; init; }
    public int? DelayMs { get; init; }
    public int? RetryCount { get; init; }
    public double? DelayMultiplier { get; init; }
    public HttpStatusCode[] RetryOn { get; init; } = [];
    public string[] ProcessorTags { get; init; } = [];
    public Dictionary<string, string> Headers { get; init; } = new();
    public Dictionary<string, string> QueryParameters { get; init; } = new();
}

public sealed class CrawlerJobController(ICrawler crawler) : WebApiController
{
    [Route(HttpVerbs.Post, "/jobs")]
    public async Task<AddCrawlerJobResult> AddJob()
    {
        var data = await HttpContext.GetRequestDataAsync<AddCrawlerJobRequest>();
        var crawlJob = new FetcherContext(data.Url, new HttpMethod(data.Method), data.RetryCount ?? 0, 
            data.DelayMs ?? 0, data.DelayMultiplier ?? 1, data.RetryOn);
        crawlJob.WithProcessorTags(data.ProcessorTags);
        foreach (var (key, value) in data.Headers) crawlJob.WithHeader(key, value);
        foreach (var (key, value) in data.QueryParameters) crawlJob.WithQuery(key, value);
        var result = crawler.AddCrawlJob(crawlJob);
        return new AddCrawlerJobResult { Success = result };
    }

}