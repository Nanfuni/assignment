using Newtonsoft.Json;

namespace Assignment1.Models
{
    public class Answer
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public string? QuestionId { get; set; }
        public string? Response { get; set; }
    }
}
