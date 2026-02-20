using Arachne.Abstractions.Models.Fetcher;

namespace Arachne.Abstractions.Interfaces.PageProcessor;

public interface IPageProcessor
{
    /// <summary>
    /// Checks if a fetch result can be processed by this processor.
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    bool CanProcess(FetcherResult result);

    /// <summary>
    /// Processes the fetched result asynchronously using the specified processor.
    /// </summary>
    /// <param name="result">The fetcher result containing the page content and status code.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ProcessPageAsync(FetcherResult result, CancellationToken cancellationToken = default);
}