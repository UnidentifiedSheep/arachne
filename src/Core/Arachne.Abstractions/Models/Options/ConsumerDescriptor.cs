namespace Arachne.Abstractions.Models.Options;

public sealed class ConsumerDescriptor<TTransportOptions>
{
    public required Type EventType { get; init; }
    public required Type ConsumerType { get; init; }
    public required ConsumerOptions<TTransportOptions> Options { get; init; }
}