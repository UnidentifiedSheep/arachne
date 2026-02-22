using Arachne.Contracts.Events;
using MassTransit;

namespace Processor.Consumers;

public class FetchCompletedConsumer : IConsumer<FetchCompletedEvent>
{
    public Task Consume(ConsumeContext<FetchCompletedEvent> context)
    {
        throw new NotImplementedException();
    }
}