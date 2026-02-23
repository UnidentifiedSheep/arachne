namespace Arachne.Abstractions.Models.Options;

public class ResponseDispatcherOptions
{
    public int MaxConcurrencyPerDispatch
    {
        get;
        init
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);
            field = value;
        }
    } = 5;
}