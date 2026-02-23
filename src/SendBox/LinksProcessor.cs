using System.Text.RegularExpressions;
using Arachne.Abstractions.Interfaces.Processor;
using Arachne.Abstractions.Models.Fetcher;
using Arachne.Contracts.Events;
using MassTransit;

using FetcherContext = Arachne.Contracts.Models.FetcherContext;

namespace SendBox;

public class LinksProcessor(IPublishEndpoint publishEndpoint) : IResponseProcessor
{
    public bool CanProcess(ReadonlyFetcherResult result) => true;

    public async Task ProcessResponseAsync(ReadonlyFetcherResult result, CancellationToken cancellationToken = default)
    {
        if (result.Result == null) return;
        var links = ProcessHtml(result.Result);

        await publishEndpoint.Publish(new AddCrawlJobEvent
        {
            Jobs = links.Select(x => new FetcherContext
            {
                Id = Guid.NewGuid(),
                Url = x,
                Method = HttpMethod.Get.Method,
                ProcessorTags = ["links"]
            }).ToList()
        }, cancellationToken);
    }
    
    private static readonly Regex AnchorHrefRegex = new(
        @"<a\b[^>]*?\bhref\s*=\s*(?:""(?<url>[^""]*)""|'(?<url>[^']*)'|(?<url>[^\s>]+))",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private string[] ProcessHtml(string html)
    {
        if (string.IsNullOrWhiteSpace(html))
            return Array.Empty<string>();

        var matches = AnchorHrefRegex.Matches(html);

        var result = new List<string>(matches.Count);

        foreach (Match match in matches)
        {
            var url = match.Groups["url"].Value;
            if (!string.IsNullOrWhiteSpace(url))
                result.Add(url);
        }

        return result.ToArray();
    }
}