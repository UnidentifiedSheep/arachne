using Arachne.Abstractions.Interfaces.Processor;
using Arachne.Abstractions.Models.Fetcher;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Processor;

public class ResponseDispatcher(IServiceProvider sp, ITagContainer tagContainer, ILogger<ResponseDispatcher> logger) 
    : IResponseDispatcher
{
    public async Task DispatchAsync(FetcherResult result)
    {
        var processors = tagContainer.GetProcessors(result.Context.ProcessorTags);
        foreach (var processorType in processors)
        {
            await using var scope = sp.CreateAsyncScope();
            object? processorObj = scope.ServiceProvider.GetService(processorType);
            if (processorObj is IResponseProcessor processor && processor.CanProcess(result))
                await processor.ProcessResponseAsync(result);
            else
                logger.LogWarning("Processor {processor} cannot process response, at {dt}.", processorType.Name, 
                    DateTime.UtcNow);
        }
    }
}