using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Ofl.Twitch
{
    public interface ITwitchClient
    {
        Task<ResponseWrapper<PagedResponse<ReadOnlyCollection<Video>>>> GetVideosByIdAsync(
            IEnumerable<string> videoIds, 
            CancellationToken cancellationToken
        );
    }
}
