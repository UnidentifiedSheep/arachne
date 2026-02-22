using Arachne.Abstractions.Models.Fetcher;

namespace Arachne.Contracts.Events;

public class FetchCompletedEvent
{
    public FetcherResult Result { get; init; } = null!;
}