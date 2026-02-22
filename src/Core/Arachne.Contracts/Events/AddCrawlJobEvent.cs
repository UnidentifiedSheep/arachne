using Arachne.Abstractions.Models.Fetcher;

namespace Arachne.Contracts.Events;

public class AddCrawlJobEvent
{
    public List<FetcherContext> Jobs = [];
}