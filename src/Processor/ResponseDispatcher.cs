using Arachne.Abstractions.Interfaces.Processor;
using Arachne.Abstractions.Models.Fetcher;
using Arachne.Abstractions.Models.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Processor;

public class ResponseDispatcher(IServiceProvider sp, ITagContainer tagContainer, ILogger<ResponseDispatcher> logger,
    ResponseDispatcherOptions options) : IResponseDispatcher
{
    public async Task DispatchAsync(ReadonlyFetcherResult result)
    {
        Queue<Type> processors = new Queue<Type>(tagContainer.GetProcessors(result.Context.ProcessorTags));
        if (processors.Count == 0)
        {
            logger.LogWarning("No processors found for response. Tags: {tg}", result.Context.ProcessorTags);
            return;
        }

        while (processors.Count > 0)
        {
            List<Task> batch = DequeueNextBatch(processors, result, options.MaxConcurrencyPerDispatch);
            await Task.WhenAll(batch);
        }
    }

    private List<Task> DequeueNextBatch(Queue<Type> processors, ReadonlyFetcherResult result, int count)
    {
        int counter = count;
        var tasks = new List<Task>(count);
        while (processors.Count > 0 && counter-- > 0)
            tasks.Add(ProcessProcessorAsync(processors.Dequeue(), result));
        return tasks;
    }
    
    private async Task ProcessProcessorAsync(Type processorType, ReadonlyFetcherResult result)
    {
        await using var scope = sp.CreateAsyncScope();
        var processorObj = scope.ServiceProvider.GetRequiredService(processorType);

        if (processorObj is IResponseProcessor processor)
        {
            if (!processor.CanProcess(result))
            {
                if (!logger.IsEnabled(LogLevel.Information)) return;
                logger.LogInformation("Processor {processor} cannot process response.", processorType.Name);
                return;
            }
            
            try
            {
                await processor.ProcessResponseAsync(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error processing response.");
            }
        }
        else
            logger.LogError("Registered processor {processor} does not implement IResponseProcessor.", processorType.Name);
    }
}