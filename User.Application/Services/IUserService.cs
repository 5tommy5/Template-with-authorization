using Users.Domain.Models;

namespace Users.Application.Services
{
    public interface IUserService
    {

        public Task<ResponseModel> Register(RegisterModel model);
        public Task<LoginInfoModel> Login(LoginModel model);
        public Task<LoginInfoModel> Login(ExternalLoginModel model);


        public Task ForgetPassword(ForgetPasswordModel model);
        public Task<ResponseModel> RecoverPassword(RecoverPasswordModel model);

        public Task<ResponseModel> ConfirmEmail(string confirmationToken);
        public Task ResendConfirmationCode(ResendConfirmationCodeModel model);

        public Task<LoginInfoModel> RevokeRefreshToken(string refreshToken);
    }
}
