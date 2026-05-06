using Newtonsoft.Json;

namespace ASPBackendApp.Contracts
{
    public record PostDto
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; init; }

        [JsonProperty(PropertyName = "content")]
        public string Content { get; init; }

        [JsonProperty(PropertyName = "author")]
        public string Author { get; init; }
    }


}
