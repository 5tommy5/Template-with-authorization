using EmailService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Template.Domain.Configuration;
using Template.Infrastructure;
using Users.Application.Services;
using Users.Domain;
using Users.Domain.Configuration;
using Users.Domain.Entities;
using Users.Domain.Models;

namespace Users.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly ApplicationContext _db;
        private readonly JwtConfiguration _jwtConfig;
        private readonly IEmailSender _emailSender;
        private readonly SendGridTemplates _templates;
        public UserService(ITokenService tokenService, ApplicationContext db, IOptions<JwtConfiguration> jwtConfig, IEmailSender sender, IOptions<SendGridTemplates> templates)
        {
            _tokenService = tokenService;
            _db = db;
            _jwtConfig = jwtConfig.Value;
            _emailSender = sender;
            _templates = templates.Value;
        }




        public async Task ForgetPassword(ForgetPasswordModel model)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

            if(user is null)
            {
                return ;
            }

            user.ForgetToken = _tokenService.CreateRefreshToken();
            user.ForgetTokenExpiryTime = DateTime.Now.AddMinutes(15);

            await _db.SaveChangesAsync();

            var callbackUri = model.ClientURI + $"?token={user.ForgetToken}";

            _templates.Templates.TryGetValue("ForgetPass", out string? template);

            if(template is null)
            {
                throw new Exception("There is not such sendgrid template");
            }

            await _emailSender.Send(model.Email, template, new
            {
                callback = callbackUri
            });
        }

        public async Task<LoginInfoModel> Login(LoginModel model)
        {
            var user = await _db.Users.Include(x=> x.Role).FirstOrDefaultAsync(x => x.Email == model.Email && x.Password == HashService.Hash(model.Password) );

            if(user is null)
            {
                return new LoginInfoModel("Uncorrect credentials");
            }
            if (!user.IsConfirmed.Value)
            {
                return new LoginInfoModel("Account email is not confirmed");
            }

            var accessToken = _tokenService.CreateAccessToken(user);
            var refreshToken = _tokenService.CreateRefreshToken();

            user.RefreshToken = refreshToken;
            user.ExpiryTime = DateTime.Now.AddDays(_jwtConfig.RefreshTokenValidityInDays);

            await _db.SaveChangesAsync();

            return new LoginInfoModel(user.Username, new TokenModel() { AccessToken = accessToken, RefreshToken = refreshToken});
        }

        public async Task<LoginInfoModel> Login(ExternalLoginModel model)
        {
            var payload = await _tokenService.VerifyGoogleToken(model);

            if(payload is null || payload.Email is null)
            {
                return new LoginInfoModel("Something gone wrong");
            }

            var user = await _db.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Email == payload.Email);

            if (user is null)
            {
                var newUser = new User()
                {
                    Email = payload.Email,
                    IsConfirmed = true,
                    RoleId = ((int)RolesHelper.Viewer),
                    Username = payload.GivenName,
                    RefreshToken = _tokenService.CreateRefreshToken(),
                    ExpiryTime = DateTime.Now.AddDays(_jwtConfig.RefreshTokenValidityInDays),
                    Password = null
                };

                await _db.Users.AddAsync(newUser);
                await _db.SaveChangesAsync();

                var accessTokenN = _tokenService.CreateAccessToken(newUser);

                return new LoginInfoModel(newUser.Username, new TokenModel() { AccessToken = accessTokenN, RefreshToken = newUser.RefreshToken });
            }

            if(user.Password is not null)
            {
                return new LoginInfoModel("Use email and password to login");
            }

            var accessToken = _tokenService.CreateAccessToken(user);
            var refreshToken = _tokenService.CreateRefreshToken();

            user.RefreshToken = refreshToken;
            user.ExpiryTime = DateTime.Now.AddDays(_jwtConfig.RefreshTokenValidityInDays);

            await _db.SaveChangesAsync();

            return new LoginInfoModel(user.Username, new TokenModel() { AccessToken = accessToken, RefreshToken = refreshToken });

        }

        public async Task<ResponseModel> RecoverPassword(RecoverPasswordModel model)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.ForgetToken == model.RecoveryToken && x.ForgetTokenExpiryTime > DateTime.Now);
            var res = new ResponseModel();

            if (user is null)
            {
                res.Errors?.Append("Could not identify user");
                return res;
            }

            if(!ValidatePass(res, model.Password))
            {
                return res;
            }

            user.Password = HashService.Hash(model.Password);
            await _db.SaveChangesAsync();

            res.Success = true;
            return res;
        }

        public async Task<ResponseModel> Register(RegisterModel model)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
            var res = new ResponseModel();
            if(user is not null)
            {
                res.Errors?.Append("User with such email already exists");
                return res;
            }

            if (!ValidatePass(res, model.Password))
            {
                return res;
            }

            var newUser = new User()
            {
                Email = model.Email,
                IsConfirmed = false,
                RoleId = ((int)RolesHelper.Viewer),
                Username = model.Username,
                Password = HashService.Hash(model.Password)
            };

            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();


            var callbackUri = model.ClientURI + $"?token={newUser.Id}";

            _templates.Templates.TryGetValue("ConfirmEmail", out string? template);

            if (template is null)
            {
                throw new Exception("There is not such sendgrid template");
            }

            await _emailSender.Send(model.Email, template, new
            {
                callback = callbackUri
            });

            res.Success = true;
            return res;

        }

        public async Task<ResponseModel> ConfirmEmail(string confirmationToken)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id.Equals(new Guid(confirmationToken)));

            user.IsConfirmed = true;
            await _db.SaveChangesAsync();

            var res = new ResponseModel();
            res.Success = true;

            return res;
        }

        public async Task ResendConfirmationCode(ResendConfirmationCodeModel model)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

            if(user is null)
            {
                return;
            }

            var callbackUri = model.ClientUri + $"?token={user?.Id}";

            _templates.Templates.TryGetValue("ConfirmEmail", out string? template);

            if (template is null)
            {
                throw new Exception("There is not such sendgrid template");
            }

            await _emailSender.Send(model.Email, template, new
            {
                callback = callbackUri
            });
        }


        private bool ValidatePass(ResponseModel res, string pass)
        {
            if(res is null)
            {
                res = new ResponseModel();
            }

            if (string.IsNullOrEmpty(pass))
            {
                res.Errors?.Append("Password must not be empty");
            }

            if (pass.Length < 4)
            {
                res.Errors?.Append("Password length must be more than 3 charecter");
            }

            return res.Errors.Any() ? false : true;
        }

        public async Task<LoginInfoModel> RevokeRefreshToken(string refreshToken)
        {
            var user = await _db.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

            if(user is null || user.ExpiryTime < DateTime.Now)
            {
                return new LoginInfoModel("Refresh token expired");
            }

            user.RefreshToken = _tokenService.CreateRefreshToken();
            user.ExpiryTime = DateTime.Now.AddDays(_jwtConfig.RefreshTokenValidityInDays);

            await _db.SaveChangesAsync();

            return new LoginInfoModel(user.Username, new TokenModel() { AccessToken = _tokenService.CreateAccessToken(user), RefreshToken = user.RefreshToken });

        }
    }
}
