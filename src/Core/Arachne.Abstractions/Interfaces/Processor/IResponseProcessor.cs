using Arachne.Abstractions.Models.Fetcher;

namespace Arachne.Abstractions.Interfaces.Processor;

public interface IResponseProcessor
{
    /// <summary>
    /// Checks if a fetch result can be processed by this processor.
    /// </summary>
    /// <param name="result"></param>
    /// <returns>True if can process, else returns false.</returns>
    bool CanProcess(ReadonlyFetcherResult result);

    /// <summary>
    /// Processes the fetched result asynchronously using the specified processor.
    /// </summary>
    /// <param name="result">The fetcher result containing the page content and status code.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ProcessResponseAsync(ReadonlyFetcherResult result, CancellationToken cancellationToken = default);
}