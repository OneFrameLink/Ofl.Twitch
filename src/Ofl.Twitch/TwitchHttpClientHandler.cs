using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Ofl.Net.Http;

namespace Ofl.Twitch
{
    public class TwitchHttpClientHandler : HttpClientHandler
    {
        #region Constructor

        public TwitchHttpClientHandler(IClientIdProvider clientIdProvider)
        {
            // Validate parameters.
            _clientIdProvider = clientIdProvider ?? throw new ArgumentNullException(nameof(clientIdProvider));

            // Set decompression.
            this.SetCompression();
        }

        #endregion

        #region Instance state

        private readonly IClientIdProvider _clientIdProvider;

        private string? _bearerToken;

        private readonly object _bearerTokenLock = new object();

        #endregion

        #region Helpers

        private async Task<string> GetBearerTokenAsync(
            bool refresh,
            CancellationToken cancellationToken
        )
        {
            // If there is a token, return it.  Lock access to the token
            // NOTE: This will have the potential to generate
            // multiple tokens at the same time; it is expected
            // that they expire if we generate ones that we don't need.
            // This is an intentional decision so that we don't lock
            // around the entire fetch operation.
            // If we are refreshing (because we failed) then
            // this check should be ignored.
            if (!refresh)
                // Lock access
                lock (_bearerTokenLock)
                    // If set, return.
                    if (!string.IsNullOrWhiteSpace(_bearerToken))
                        // Return.
                        return _bearerToken;

            // The URL of the ID service.
            // Sub in the parameters.
            string idServiceUrl = $"https://id.twitch.tv/oauth2/token?client_id={_clientIdProvider.ClientId}&client_secret={_clientIdProvider.ClientSecret}&grant_type=client_credentials";

            // Create a message.
            using var request = new HttpRequestMessage(HttpMethod.Post, idServiceUrl);

            // Send.
            using var response = await base
                .SendAsync(request, cancellationToken)
                .ConfigureAwait(false);

            // Get the stream.
            using var stream = await response
                .Content
                .ReadAsStreamAsync()
                .ConfigureAwait(false);

            // Get the access token from the JSON.
            using var document = await JsonDocument
                .ParseAsync(stream)
                .ConfigureAwait(false);

            // Get the access token.
            string accessToken = document.RootElement.GetProperty("access_token").GetString();

            // Set the bearer token, lock access.
            lock (_bearerTokenLock)
                _bearerToken = accessToken;

            // Return the access token.
            return accessToken;
        }

        private async Task<HttpResponseMessage> SendAuthorizedRequestAsync(
            HttpRequestMessage request,
            bool refresh,
            CancellationToken cancellationToken
        )
        {
            // Validate parameters.
            if (request == null) throw new ArgumentNullException(nameof(request));

            // Get the bearer token.
            string bearerToken = await GetBearerTokenAsync(refresh, cancellationToken)
                .ConfigureAwait(false);

            // Overwrite the client ID and the bearer token every time.
            request.Headers.Remove("Client-ID");
            request.Headers.Add("Client-ID", _clientIdProvider.ClientId);
            request.Headers.Remove("Authorization");
            request.Headers.Add("Authorization", $"Bearer {bearerToken}");

            // Return the response.
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }


        #endregion

        #region Overrides of HttpClientHandler

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, 
            CancellationToken cancellationToken
        )
        {
            // Validate parameters.
            if (request == null) throw new ArgumentNullException(nameof(request));

            // Send an authorized request.
            HttpResponseMessage response = await SendAuthorizedRequestAsync(request, false, cancellationToken)
                .ConfigureAwait(false);

            // If this doesn't specifically fail authorization, return
            // the message.
            if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
                return response;

            // We need to force a refresh, dispose the response.
            using (response) { }

            // Send the request again, force a refresh.
            return await SendAuthorizedRequestAsync(request, true, cancellationToken)
                .ConfigureAwait(false);
        }

        #endregion
    }
}
