using System.Runtime.CompilerServices;

namespace FairwayAPI.Utils
{
    /*
     * This code was taken from a blog post
     * Date Used: 9 November 2024
     * Date posted: 11 November 2021
     * Author: Siderite
     * Source: https://siderite.dev/blog/clean-way-to-build-urls-in-net#at2416791687
     */
    public static class UrlHelper
        {
            public static FormattableString EncodeUrlParameters(FormattableString url)
             {
                return FormattableStringFactory.Create(
                    url.Format,
                    url.GetArguments()
                        .Select(a => Uri.EscapeDataString(a?.ToString() ?? ""))
                        .ToArray());
             }
    }
    
}
