using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Ofl.Net.Http.ApiClient.Json;
using Ofl.Text.Json;
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
                PropertyNamingPolicy = new SnakeCaseJsonNamingPolicy(true)
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

            // Add the parameters.  Use a query builder.
            var builder = new QueryBuilder
                { { "id", ids } };

            // Create the url.
            string url = $"/helix/videos{builder.ToQueryString()}";

            // Make the call.
            return GetAsync<
                PagedResponse<Video[]>, 
                ResponseWrapper<PagedResponse<ReadOnlyCollection<Video>>>
            >(
                url,
                WrapResponse,
                cancellationToken
            );
        }

        #endregion

        #region Helpers

        private static ResponseWrapper<PagedResponse<ReadOnlyCollection<TResponse>>> WrapResponse<TResponse>(
            HttpResponseMessage message,
            PagedResponse<TResponse[]> response
        )
        {
            // Validate parameters.
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (response == null) throw new ArgumentNullException(nameof(response));

            long? ParseHeader(string header) {
                // Validate parameters.
                if (string.IsNullOrWhiteSpace(header))
                    throw new ArgumentNullException(nameof(header));

                // Try to get the values.
                if (!message.Headers.TryGetValues(header, out IEnumerable<string> values))
                    // Return null.
                    return null;

                // Get the enumerator.
                using IEnumerator<string> enumerator = values.GetEnumerator();

                // Move to the first item, if there is nothing, return null.
                if (!enumerator.MoveNext()) return null;

                // There is an item, store it.
                string value = enumerator.Current;

                // Is there another value after this?  If so, return
                // null.
                if (enumerator.MoveNext()) return null;

                // Try and parse the value.
                return long.TryParse(value, out long l)
                    ? l
                    : (long?) null;
            }

            // Get the items from the header.
            // From: https://dev.twitch.tv/docs/api/guide#rate-limits
            int limit = (int) (ParseHeader("Ratelimit-Limit") ?? -1);
            int remaining = (int) (ParseHeader("Ratelimit-Remaining") ?? -1);
            long reset = ParseHeader("Ratelimit-Reset") ?? -1;

            // Return.
            return new ResponseWrapper<PagedResponse<ReadOnlyCollection<TResponse>>>(
                new RateLimit(limit, remaining, DateTimeOffset.FromUnixTimeSeconds(reset)), 
                new PagedResponse<ReadOnlyCollection<TResponse>> {
                    Data = new ReadOnlyCollection<TResponse>(response.Data)
                }
            );
        }

        #endregion
    }
}
