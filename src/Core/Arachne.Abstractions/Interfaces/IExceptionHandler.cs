namespace Arachne.Abstractions.Interfaces;

public interface IExceptionHandler
{
    Task HandleAsync(Exception exception);
}