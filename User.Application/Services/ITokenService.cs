using Google.Apis.Auth;
using Users.Domain.Entities;
using Users.Domain.Models;

namespace Users.Application.Services
{
    public interface ITokenService
    {
        public string CreateAccessToken(User user);
        public string CreateRefreshToken();
        string? GetEmailFromExpiredToken(string token);
        Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(ExternalLoginModel model);
    }
}
