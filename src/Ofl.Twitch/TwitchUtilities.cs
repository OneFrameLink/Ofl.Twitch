using System;
using System.Text.RegularExpressions;

namespace Ofl.Twitch
{
    public class TwitchUtilities : ITwitchUtilities
    {
        #region Implementation of ITwitchUtilities

        private static readonly Regex VideoIdRegex = new Regex(@"^\/videos\/(?<videoId>[0-9]+)$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex HostRegex = new Regex(@"^(.*\.)?twitch.tv$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public VideoUrl ParseVideoUrl(string url)
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(url)) return null;

            // The Uri.
            Uri uri;

            // Try and parse the URL.
            if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri)) return null;

            // If the host doesn't match, return null.
            if (!HostRegex.IsMatch(uri.Host)) return null;

            // The ParsedUrl.
            var parsedUrl = new VideoUrl();

            // The video ID match.
            Match match = VideoIdRegex.Match(uri.AbsolutePath);

            // If it isn't a success, return the parsed URL.
            if (!match.Success) return parsedUrl;

            // Set the channel and video ID.
            parsedUrl.VideoId = match.Groups["videoId"].Value;

            // Return the parsed URL.
            return parsedUrl;
        }

        #endregion
    }
}
