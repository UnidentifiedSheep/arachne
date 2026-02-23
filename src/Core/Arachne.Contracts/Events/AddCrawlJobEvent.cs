

using Arachne.Contracts.Models;

namespace Arachne.Contracts.Events;

public class AddCrawlJobEvent
{
    public required IReadOnlyCollection<FetcherContext> Jobs { get; init; }
}