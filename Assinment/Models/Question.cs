using Newtonsoft.Json;

namespace Assignment1.Models
{
    public class Question
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public List<string> Options { get; set; }
    }
}


