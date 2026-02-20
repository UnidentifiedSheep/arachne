namespace Arachne.Abstractions.Interfaces.Fetcher.Pipeline;

public interface IMiddlewareRegistrator
{
    IEnumerable<Type> RegisteredMiddlewares { get; }
    
    /// <summary>
    /// Registers middleware for fetcher pipeline.
    /// </summary>
    /// <typeparam name="T">The middleware which inherits from IFetcherMiddleware</typeparam>
    void Register<T>() where T : IFetcherMiddleware;
    
    /// <summary>
    /// Finalizes the registration process for middleware in the fetcher pipeline.
    /// This method should be called after all middleware has been registered to indicate
    /// that the middleware pipeline setup is complete.
    /// </summary>
    void EndRegistration();
}