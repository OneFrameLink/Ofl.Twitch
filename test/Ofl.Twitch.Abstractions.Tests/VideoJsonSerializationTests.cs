using System;
using System.Text.Json;
using Xunit;

namespace Ofl.Twitch.Tests
{
    public class VideoJsonSerializationTests
    {
        #region Tests

        [Theory]
        [InlineData("1w1d1h1m1s", "8.1:1:1")]
        [InlineData("1s1w1d1h1m", "8.1:1:1")]
        [InlineData("1w8d25h61m61s", "16.2:2:1")]
        [InlineData("1s", "0:0:1")]
        public void Test_ParseDuration(string duration, string timespan)
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(duration)) throw new ArgumentNullException(nameof(duration));
            if (string.IsNullOrWhiteSpace(timespan)) throw new ArgumentNullException(nameof(timespan));

            // Get the expected value.
            TimeSpan expected = TimeSpan.Parse(timespan);

            // Create the JSON.
            string json = $"{{ \"{nameof(Video.Duration)}\": \"{duration}\" }}";

            // Parse.
            Video video = JsonSerializer.Deserialize<Video>(json);

            // Get the actual video.
            TimeSpan actual = video.Duration;

            // Compare.
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
