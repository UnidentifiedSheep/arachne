namespace Arachne.Abstractions.Interfaces.Crawler;

public interface IRateLimiter
{
    /// <summary>
    /// Max allowed requests per second.
    /// </summary>
    int MaxRps { get; }
    
    /// <summary>
    /// Current requests per second.
    /// </summary>
    int CurrentRps { get; }
    
    /// <summary>
    /// Changes max allowed requests per second.
    /// </summary>
    /// <param name="newRps">New rps value. Must be greater than 0.</param>
    void ChangeRps(int newRps);
    
    /// <summary>
    /// Waits till request is allowed to execute.
    /// If current RPS is greater than max allowed RPS, this method will wait till next allowed request.
    /// </summary>
    /// <returns></returns>
    Task WaitTillAllowed();
}