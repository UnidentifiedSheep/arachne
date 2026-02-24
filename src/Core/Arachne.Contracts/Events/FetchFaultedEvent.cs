using Arachne.Contracts.Models;

namespace Arachne.Contracts.Events;

public class FetchFaultedEvent
{
    public FetcherContext? Context { get; init; }
    public required string Exception { get; init; }
}