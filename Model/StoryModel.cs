using Newtonsoft.Json;

namespace Quantc.StoriesWebAPI.Model
{
    public class StoryModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("url")]
        public string? URI { get; set; }

        [JsonProperty("by")]
        public string? PostedBy { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("score")]
        public int Score { get; set; }

        [JsonProperty("descendants")]
        public int CommentCount { get; set; }
    }
}