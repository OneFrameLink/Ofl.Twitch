using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ofl.Twitch
{
    public static class TwitchClientExtensions
    {
        public static Task<ResponseWrapper<PagedResponse<ReadOnlyCollection<Video>>>> GetVideosByIdAsync(
            this ITwitchClient client,
            CancellationToken cancellationToken,
            params string[] ids
        )
        {
            // Validate parameters.
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (ids == null) throw new ArgumentNullException(nameof(ids));

            // Call the client
            return client
                .GetVideosByIdAsync(ids.AsEnumerable(), cancellationToken);
        }

        public static async Task<Video?> GetVideoByIdAsync(
            this ITwitchClient client,
            string id,
            CancellationToken cancellationToken
        )
        {
            // Validate parameters.
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));

            // Call the client, get the response.
            ResponseWrapper<PagedResponse<ReadOnlyCollection<Video>>> response = await client
                .GetVideosByIdAsync(cancellationToken, id)
                .ConfigureAwait(false);

            // Return the single item or default.
            return response.Response.Data.SingleOrDefault();
        }
    }
}
