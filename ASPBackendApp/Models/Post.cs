using Newtonsoft.Json;

namespace ASPBackendApp.Models
{
    public class Post
    {
        [JsonProperty("id")]   // must match /id in Cosmos
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }
    }
}
