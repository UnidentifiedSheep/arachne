using Arachne.Abstractions.Models.Fetcher;

namespace Arachne.Contracts.Events;

public class FetchFaultedEvent
{
    public FetcherContext? Context { get; init; }
    public Exception Exception { get; init; } = null!;
}