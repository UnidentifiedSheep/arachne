using Arachne.Contracts.Models;

namespace Arachne.Contracts.Events;

public class FetchCompletedEvent
{
    public required FetcherResult Result { get; init; }
}