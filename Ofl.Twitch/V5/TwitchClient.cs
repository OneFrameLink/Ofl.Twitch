using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ofl.Core.Net.Http;

namespace Ofl.Twitch.V5
{
    public class TwitchClient : JsonApiClient, ITwitchClient
    {
        #region Constructor

        public TwitchClient(
            IClientIdProvider clientIdProvider,
            IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            // Validate parameters.
            if (clientIdProvider == null) throw new ArgumentNullException(nameof(clientIdProvider));

            // Assign values.
            _clientIdProvider = clientIdProvider;
        }

        #endregion

        #region Instance, read-only state.

        private readonly IClientIdProvider _clientIdProvider;

        #endregion

        #region Overrides

        protected override Task<HttpClient> CreateHttpClientAsync(CancellationToken cancellationToken)
        {
            // Call the overload, returning the handler.
            return CreateHttpClientAsync(new TwitchHttpClientHandler(_clientIdProvider), cancellationToken);
        }

        protected override Task<string> FormatUrlAsync(string url, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException(nameof(url));

            // The base.
            const string baseUri = "https://api.twitch.tv";

            // Return the path.
            return Task.FromResult(baseUri + url);
        }

        #endregion

        #region Implementation of ITwitchClient

        public Task<Video> GetVideo(string id, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));

            // The url.
            string url = $"/kraken/videos/{id}";

            // Get the stuff.
            return GetAsync<Video>(url, cancellationToken);
        }

        #endregion
    }
}
