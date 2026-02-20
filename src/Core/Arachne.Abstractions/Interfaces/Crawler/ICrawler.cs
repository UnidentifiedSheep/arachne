using Arachne.Abstractions.EventArgs;
using Arachne.Abstractions.Models.Fetcher;

namespace Arachne.Abstractions.Interfaces.Crawler;

public interface ICrawler
{
    /// <summary>
    /// Starts crawling process.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task StartAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Stops crawling process.
    /// </summary>
    /// <returns></returns>
    Task StopAsync();

    /// <summary>
    /// Adds a crawl job to the queue and starts a crawling process if not started yet.
    /// </summary>
    /// <param name="context">Context for fetch request.</param>
    /// <returns>True if a job was added, otherwise false.</returns>
    bool AddCrawlJob(FetcherContext context);
    
    /// <summary>
    /// Fired when fetch is completed.
    /// </summary>
    event EventHandler<FetcherResultEventArgs> OnFetchCompleted;
    
    /// <summary>
    /// Fired when fetch failed.
    /// </summary>
    event EventHandler<FetcherFaultEventArgs> OnFetchFaulted;
}