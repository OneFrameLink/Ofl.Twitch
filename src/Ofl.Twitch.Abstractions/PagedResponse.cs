namespace Ofl.Twitch
{
    public class PagedResponse<T>
    {
        public T Data { get; set; } = default!;

        public Pagination Pagination { get; set; } = default!;
    }
}
