using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Ofl.Net.Http.ApiClient.Json;
using Ofl.Threading.Tasks;

namespace Ofl.Twitch
{
    public class TwitchClient : JsonApiClient, ITwitchClient
    {
        #region Constructor

        public TwitchClient(
            HttpClient httpClient) : base(httpClient)
        { }

        #endregion

        #region Overrides

        protected override JsonSerializerOptions CreateJsonSerializerOptions() =>
            new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

        protected override ValueTask<string> FormatUrlAsync(string url, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException(nameof(url));

            // The base.
            const string baseUri = "https://api.twitch.tv";

            // Return the path.
            return ValueTaskExtensions.FromResult(baseUri + url);
        }

        #endregion

        #region Implementation of ITwitchClient

        public Task<ResponseWrapper<PagedResponse<ReadOnlyCollection<Video>>>> GetVideosByIdAsync(
            IEnumerable<string> ids, 
            CancellationToken cancellationToken
        )
        {
            // Validate parameters.
            if (ids == null) throw new ArgumentNullException(nameof(ids));

            // The url.
            string url = $"/helix/videos";

            // Add the parameters.
            
            // Make the call

            // Get the stuff.
            return GetAsync<Video>(url, cancellationToken);
        }

        protected override Task<TResponse> ProcessResponseAsync<TResponse>(
            HttpResponseMessage httpResponseMessage, 
            JsonSerializerOptions jsonSerializerOptions, 
            CancellationToken 
            cancellationToken
        )
        {
            return base.ProcessResponseAsync<TResponse>(httpResponseMessage, jsonSerializerOptions, cancellationToken);
        }

        #endregion
    }
}
