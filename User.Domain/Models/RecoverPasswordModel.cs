using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Users.Domain.Models
{
    public class RecoverPasswordModel
    {
        [Required(ErrorMessage = "Password is required")]
        [JsonPropertyName("password")]
        public string? Password { get; set; }

        [JsonPropertyName("recoveryToken")]
        public string? RecoveryToken { get; set; }
    }
}
