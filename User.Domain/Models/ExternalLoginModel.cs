using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Users.Domain.Models
{
    public class ExternalLoginModel
    {
        [Required]
        [JsonPropertyName("provider")]
        public string? Provider { get; set; }

        [Required]
        [JsonPropertyName("token")]
        public string? IdToken { get; set; }
    }
}
