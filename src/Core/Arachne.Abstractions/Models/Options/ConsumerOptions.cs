namespace Arachne.Abstractions.Models.Options;

public sealed class ConsumerOptions<TTransportOptions>(TTransportOptions transportOptions) : ConsumerOptions
{
    public TTransportOptions TransportOptions { get; init; } = transportOptions;
}

public class ConsumerOptions
{
    public string? EndpointName { get; set; }
    public int RetryCount { get; set; }
}