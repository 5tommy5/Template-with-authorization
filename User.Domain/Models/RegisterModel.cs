using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Users.Domain.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "User Name is required")]
        [JsonPropertyName("username")]
        public string? Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(5, ErrorMessage = "Password length must be minimum 5 chars")]
        [JsonPropertyName("password")]
        public string? Password { get; set; }



        [Required]
        [JsonPropertyName("clientUri")]
        public string ClientURI { get; set; }
    }
}
