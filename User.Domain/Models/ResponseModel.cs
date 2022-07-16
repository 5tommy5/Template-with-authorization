using System.Text.Json.Serialization;

namespace Users.Domain.Models
{
    public class ResponseModel
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("errors")]
        public IEnumerable<string>? Errors { get; set; }

        public ResponseModel()
        {
            Success = false;
            Errors = new List<string>();
        }
    }
}
