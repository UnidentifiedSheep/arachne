using System.Threading.Channels;
using Arachne.Abstractions.Models.Fetcher;

namespace Arachne.Abstractions.Interfaces.Crawler;

public interface ICrawler : IAsyncDisposable
{
    ChannelReader<FetcherContext> Reader { get; }

    /// <summary>
    /// Adds a crawl job to the queue and starts a crawling process if not started yet.
    /// </summary>
    /// <param name="context">Context for fetch request.</param>
    /// <returns>True if a job was added, otherwise false.</returns>
    ValueTask<(bool, Guid)> AddCrawlJob(FetcherContext context);
}