using Arachne.Abstractions.Interfaces.Processor;
using Arachne.Abstractions.Models.Fetcher;
using Arachne.Contracts.Events;
using Arachne.Extensions;
using MassTransit;

namespace Processor.Consumers;

public class FetchCompletedConsumer(IResponseDispatcher dispatcher) : IConsumer<FetchCompletedEvent>
{
    public async Task Consume(ConsumeContext<FetchCompletedEvent> context)
    {
        FetcherResult fetchResult = context.Message.Result.ToModel();
        await dispatcher.DispatchAsync(fetchResult);
    }
}