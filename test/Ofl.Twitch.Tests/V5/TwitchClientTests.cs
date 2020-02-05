using Ofl.Twitch.V5;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Ofl.Twitch.Tests.V5
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
        [InlineData("106400740")]
        public async Task Test_GetVideo_Async(string videoId)
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(videoId))
                throw new ArgumentNullException(nameof(videoId));

            // Create the client.
            ITwitchClient client = CreateTwitchClient();

            // Make the call.
            Video response = await client
                .GetVideo(videoId, CancellationToken.None)
                .ConfigureAwait(false);

            // Assert.
            Assert.NotNull(response.Url);
        }

        #endregion
    }
}
