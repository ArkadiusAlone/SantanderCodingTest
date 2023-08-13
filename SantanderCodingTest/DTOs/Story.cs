using Newtonsoft.Json;
using NSwag.Annotations;
using SantanderCodingTest.Helpers;

namespace SantanderCodingTest.DTOs
{
    public class Story
    {
        public string? Title { get; set; }

        [JsonProperty(PropertyName = "Url")]
        public string? Uri { get; set; }

        [JsonProperty(PropertyName ="By")]
        public string? PostedBy { get; set; }

        [JsonConverter(typeof(StoryTimestampConverter))]
        public DateTimeOffset? Time { get; set; }

        public int? Score { get; set; }

        [JsonProperty(PropertyName = "Descendants")]
        public int? CommentCount { get; set; }
    }
}
