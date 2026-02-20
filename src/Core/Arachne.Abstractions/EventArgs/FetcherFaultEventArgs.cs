namespace Arachne.Abstractions.EventArgs;

public class FetcherFaultEventArgs(Exception exception) : System.EventArgs
{
    public Exception Exception { get; } = exception;
}