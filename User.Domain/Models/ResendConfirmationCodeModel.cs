using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Users.Domain.Models
{
    public class ResendConfirmationCodeModel
    {

        [Required]
        [JsonPropertyName("clientUri")]
        public string ClientUri { get; set; }


        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
