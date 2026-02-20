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
    public int MaxConcurrency 
    { 
        get;
        init
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);
            field = value;
        }
    } = 1;
}