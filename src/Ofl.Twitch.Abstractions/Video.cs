using System;
using System.Text.Json.Serialization;

namespace Ofl.Twitch
{
    public class Video
    {
        public DateTimeOffset CreatedAt { get; set; }

        public string Description { get; set; } = default!;

        [JsonConverter(typeof(DurationJsonConverter))]
        public TimeSpan Duration { get; set; }

        public string Id { get; set; } = default!;

        public string Language { get; set; } = default!;

        public DateTimeOffset PublishedAt { get; set; }

        public string ThumbnailUrl { get; set; } = default!;

        public string Title { get; set; } = default!;

        public string Type { get; set; } = default!;

        public string Url { get; set; } = default!;

        public string UserId { get; set; } = default!;

        public string UserName { get; set; } = default!;

        public int ViewCount { get; set; }

        public string Viewable { get; set; } = default!;
    }
}
