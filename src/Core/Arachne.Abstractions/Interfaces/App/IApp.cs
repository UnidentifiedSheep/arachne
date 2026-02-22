namespace Arachne.Abstractions.Interfaces.App;

public interface IApp : IAsyncDisposable
{
    IServiceProvider Services { get; }
    Task RunAsync(CancellationToken cancellationToken = default);
    void WithExceptionHandler<THandler>() where THandler : IExceptionHandler;
    void WithExceptionHandler(Action<Exception> handler);
    void WithExceptionHandler(Func<Exception, Task> handler);
}