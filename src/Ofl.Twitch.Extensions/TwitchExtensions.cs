using System;
using System.Text.RegularExpressions;

namespace Ofl.Twitch
{
    public static class TwitchExtensions
    {
        private static readonly Regex VideoIdRegex = new Regex(@"^\/videos\/(?<videoId>[0-9]+)$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex HostRegex = new Regex(@"^(.*\.)?twitch.tv$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static string? ParseVideoId(string? url)
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(url)) return null;

            // Try and parse the URL.
            if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out Uri uri)) return null;

            // If the host doesn't match, return null.
            if (!HostRegex.IsMatch(uri.Host)) return null;

            // The video ID match.
            Match match = VideoIdRegex.Match(uri.AbsolutePath);

            // If it isn't a success, return the parsed URL.
            if (!match.Success) return null;

            // Set the channel and video ID.
            return match.Groups["videoId"].Value;
        }
    }
}