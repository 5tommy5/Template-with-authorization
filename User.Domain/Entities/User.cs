using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Users.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }

        [EmailAddress]
        [Required]
        [MaxLength(350)]
        public string Email { get; set; }
        public string? Password { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? ExpiryTime { get; set; }

        public string? ForgetToken { get; set; }
        public DateTime? ForgetTokenExpiryTime { get; set; }

        public bool? IsConfirmed { get; set; }


        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public DateTime Created { get; set; }
    }
}
