using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ofl.Net.Http;

namespace Ofl.Twitch.V5
{
    internal class TwitchHttpClientHandler : HttpClientHandler
    {
        #region Constructor

        internal TwitchHttpClientHandler(IClientIdProvider clientIdProvider)
        {
            // Validate parameters.
            _clientIdProvider = clientIdProvider ?? throw new ArgumentNullException(nameof(clientIdProvider));

            // Set decompression.
            this.SetCompression();
        }

        #endregion

        #region Instance, read-only state

        private readonly IClientIdProvider _clientIdProvider;

        #endregion

        #region Overrides of HttpClientHandler

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (request == null) throw new ArgumentNullException(nameof(request));

            // The client ID header.
            const string clientIdHeaderKey = "Client-ID";

            // If the client ID is not set.
            if (!request.Headers.Contains(clientIdHeaderKey))
                // Add.
                request.Headers.Add(clientIdHeaderKey,
                    await _clientIdProvider.GetClientIdAsync(cancellationToken).ConfigureAwait(false));

            // Accept Twitch.
            const string acceptHeader = "application/vnd.twitchtv.v5+json";

            // Set the accept header.
            request.Headers.Accept.ParseAdd(acceptHeader);

            // Call the base.
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        #endregion
    }
}
