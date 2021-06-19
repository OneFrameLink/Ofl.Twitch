using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Ofl.Twitch.Tests
{
    public class TwitchClientTests : IClassFixture<TwitchClientTestsFixture>
    {
        #region Constructor

        public TwitchClientTests(TwitchClientTestsFixture fixture)
        {
            // Validate parameters.
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        #endregion

        #region Instance, read-only state.

        private readonly TwitchClientTestsFixture _fixture;

        #endregion

        #region Helpers

        private ITwitchClient CreateTwitchClient() => _fixture.CreateTwitchClient();

        #endregion

        #region Tests

        [Theory]
        [InlineData("547460150")]
        [InlineData("547460150,546100800")]
        [InlineData("1057534640")]
        public async Task Test_GetVideosById_Async(string videoIdsString)
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(videoIdsString))
                throw new ArgumentNullException(nameof(videoIdsString));

            // Parse the string into ids.
            IReadOnlyCollection<string> videoIds = videoIdsString.Split(',');

            // Create the client.
            ITwitchClient client = CreateTwitchClient();

            // Make the call.
            var response = await client
                .GetVideosByIdAsync(videoIds, CancellationToken.None)
                .ConfigureAwait(false);

            // Assert the length.
            Assert.Equal(videoIds.Count, response.Response.Data.Count);
        }

        #endregion
    }
}
