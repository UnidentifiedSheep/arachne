using Arachne.Abstractions.Interfaces;
using Microsoft.Extensions.Logging;

namespace Arachne.App.Base.ExceptionHandlers;

public class BasicExceptionHandler(ILogger<BasicExceptionHandler> logger) : IExceptionHandler
{
    public Task HandleAsync(Exception exception)
    {
        logger.LogError(exception, "Unhandled exception caught.");
        return Task.CompletedTask;
    }
}