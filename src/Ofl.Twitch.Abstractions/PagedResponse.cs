using System.Diagnostics.CodeAnalysis;

namespace Ofl.Twitch
{
    public class PagedResponse<T>
        where T : class
    {
        public T? Data { get; set; }

        public Pagination? Pagination { get; set; }
    }
}
