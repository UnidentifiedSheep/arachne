using Arachne.Abstractions.Models.Fetcher;

namespace Arachne.Abstractions.Interfaces.Processor;

public interface IResponseDispatcher
{
    /// <summary>
    /// Dispatches a crawler result, and starts processing it.
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    Task DispatchAsync(ReadonlyFetcherResult result);
}