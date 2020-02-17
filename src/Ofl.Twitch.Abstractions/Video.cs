using System;
using System.Text.Json.Serialization;

namespace Ofl.Twitch
{
    public class Video
    {
        public DateTimeOffset CreatedAt { get; set; }

        public string? Description { get; set; }

        [JsonConverter(typeof(DurationJsonConverter))]
        public TimeSpan Duration { get; set; }

        public string? Id { get; set; }

        public string? Language { get; set; }

        public DateTimeOffset PublishedAt { get; set; }

        public string? ThumbnailUrl { get; set; }

        public string? Title { get; set; }

        public string? Type { get; set; }

        public string? Url { get; set; }

        public string? UserId { get; set; }

        public string? UserName { get; set; }

        public int ViewCount { get; set; }

        public string? Viewable { get; set; }
    }
}
