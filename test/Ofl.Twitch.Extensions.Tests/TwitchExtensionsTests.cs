using Xunit;

namespace Ofl.Twitch.Tests
{
    public class TwitchExtensionsTests
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData("https://www.youtube.com/videos/552595327", null)]
        [InlineData("https://www.twitch.tv/552595327", null)]
        [InlineData("https://www.twitch.tv/videos", null)]
        [InlineData("https://www.twitch.tv/videos/552595327", "552595327")]
        public void Test_ParseVideoId(string? url, string? expected)
        {
            // Parse the url.
            string? actual = TwitchExtensions.ParseVideoId(url);

            // Assert.
            Assert.Equal(expected, actual);
        }
    }
}
