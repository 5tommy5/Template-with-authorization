using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Users.Domain.Models
{
    public class ForgetPasswordModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        [JsonPropertyName("email")]
        public string? Email { get; set; }


        [Required]
        [JsonPropertyName("clientUri")]
        public string ClientURI { get; set; }
    }
}
