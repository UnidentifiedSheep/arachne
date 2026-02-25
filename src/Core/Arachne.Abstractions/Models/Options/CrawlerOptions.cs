namespace Arachne.Abstractions.Models.Options;

public class CrawlerOptions
{
    public int MaxRps
    {
        get;
        init
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);
            field = value;
        }
    } = int.MaxValue;
    public int WorkerCount 
    { 
        get;
        init
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);
            field = value;
        }
    } = 5;
}