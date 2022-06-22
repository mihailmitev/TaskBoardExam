using System.Text.Json.Serialization;

namespace TaskBoard.APITests
{
    public class Tasks
    {
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; }
        [JsonPropertyName("title")]
        public string title { get; set; }
        [JsonPropertyName("description")]
        public string description { get; set; }



    }
}
