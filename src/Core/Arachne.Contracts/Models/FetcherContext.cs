using System.Net;
using System.Text;

namespace Arachne.Contracts.Models;

public sealed class FetcherContext
{
    public required Guid Id { get; init; }
    public required string Url { get; init; }
    public required string Method { get; init; }

    public int RetryCount { get; init; }
    public int DelayMs { get; init; } = 1;
    public double DelayMultiplier { get; init; } = 1;
    
    public IReadOnlyCollection<int> RetryOn { get; init; } = [];
    public IReadOnlyDictionary<string, string> Headers { get; init; } = new Dictionary<string, string>();
    public IReadOnlyDictionary<string, string> QueryParameters { get; init; } = new Dictionary<string, string>();
    public IReadOnlyCollection<string> ProcessorTags { get; init; } = [];
}