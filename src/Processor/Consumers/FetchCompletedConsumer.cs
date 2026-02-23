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
        ReadonlyFetcherResult fetchResult = context.Message.Result.ToReadonlyModel();
        await dispatcher.DispatchAsync(fetchResult);
    }
}