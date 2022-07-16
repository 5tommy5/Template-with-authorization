using System.Security.Cryptography;
using System.Text;

namespace Users.Infrastructure.Services
{
    public static class HashService 
    {
        public static string Hash(string key)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
