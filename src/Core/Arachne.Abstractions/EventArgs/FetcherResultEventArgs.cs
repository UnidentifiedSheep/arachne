using Arachne.Abstractions.Models.Fetcher;

namespace Arachne.Abstractions.EventArgs;

public class FetcherResultEventArgs(FetcherResult result) : System.EventArgs
{
    public FetcherResult Result { get; } = result;
}