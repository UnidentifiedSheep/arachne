using System.Collections.Immutable;

namespace Fetcher.Constants;

public static class UserAgentConstants
{
    public static readonly ImmutableArray<string> UserAgents =
    [
        // Chrome
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/118.0.5993.118 Safari/537.36",
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 13_5) AppleWebKit/605.1.15 (KHTML, like Gecko) Chrome/118.0.5993.118 Safari/605.1.15",
        
        // Firefox
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:118.0) Gecko/20100101 Firefox/118.0",
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 13.5; rv:118.0) Gecko/20100101 Firefox/118.0",

        // Edge
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/118.0.5993.118 Safari/537.36 Edg/118.0.2088.62",

        // Safari
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 13_5) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/18.1 Safari/605.1.15",

        // Mobile Chrome
        "Mozilla/5.0 (Linux; Android 13; Pixel 7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/118.0.5993.118 Mobile Safari/537.36",

        // Mobile Safari
        "Mozilla/5.0 (iPhone; CPU iPhone OS 17_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/17.0 Mobile/15E148 Safari/604.1"
    ];
}