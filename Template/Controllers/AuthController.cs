using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Users.Application.Services;
using Users.Domain.Models;

namespace Template.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _service;
        public AuthController(IUserService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ResponseModel> Register([FromBody] RegisterModel model)
        {
            return await _service.Register(model);
        }

        [HttpPost]
        public async Task<LoginInfoModel> Login([FromBody] LoginModel model)
        {
            return await _service.Login(model);
        }

        [HttpPost]
        public async Task<LoginInfoModel> ExternalLogin([FromBody] ExternalLoginModel model)
        {
            return await _service.Login(model);
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordModel model)
        {
            await _service.ForgetPassword(model);

            return Ok();
        }

        [HttpPost]
        public async Task<ResponseModel> RecoverPassword([FromBody] RecoverPasswordModel model)
        {
            return await _service.RecoverPassword(model);
        }

        [HttpPost]
        public async Task<ResponseModel> ConfirmEmail(string confirmationToken)
        {
            return await _service.ConfirmEmail(confirmationToken);
        }

        [HttpPost]
        public async Task<IActionResult> ResendConfirmationCode([FromBody] ResendConfirmationCodeModel model)
        {
            await _service.ResendConfirmationCode(model);

            return Ok();
        }

        [HttpGet]
        public async Task<LoginInfoModel> RevokeRefreshToken(string refreshToken)
        {
            return await _service.RevokeRefreshToken(refreshToken);
        }
    }
}
