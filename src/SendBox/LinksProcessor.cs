using Arachne.Abstractions.Interfaces.Processor;
using Arachne.Abstractions.Models.Fetcher;
using Arachne.Contracts.Events;
using HtmlAgilityPack;
using MassTransit;

using FetcherContext = Arachne.Contracts.Models.FetcherContext;

namespace SendBox;

public class LinksProcessor(IPublishEndpoint publishEndpoint) : IResponseProcessor
{
    public bool CanProcess(ReadonlyFetcherResult result) => true;

    public async Task ProcessResponseAsync(ReadonlyFetcherResult result, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(result.Result))
            return;

        var links = ProcessHtml(result.Result);

        if (!links.Any())
            return;

        var jobs = links.Select(x => new FetcherContext
        {
            Id = Guid.NewGuid(),
            Url = x,
            Method = HttpMethod.Get.Method,
            ProcessorTags = ["links"]
        }).ToList();

        await publishEndpoint.Publish(new AddCrawlJobEvent
        {
            Jobs = jobs
        }, cancellationToken);
    }

    private static string[] ProcessHtml(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var linkNodes = doc.DocumentNode.SelectNodes("//a[@href]");
        if (linkNodes.Count == 0) return [];

        var urls = new List<string>(linkNodes.Count);
        foreach (var node in linkNodes)
        {
            var href = node.GetAttributeValue("href", string.Empty);
            if (!string.IsNullOrWhiteSpace(href) && Uri.TryCreate(href, UriKind.Absolute, out _))
                urls.Add(href);
        }

        return urls.ToArray();
    }
}