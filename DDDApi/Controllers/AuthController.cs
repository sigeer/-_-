using DDDApplication.Contract.Auth;
using DDDApplication.Contract.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Utility.Constants;
using Utility.Extensions;

namespace DDDApi.Controllers
{
    [AllowAnonymous]
    [Route("auth/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly IAuthService _authService;
        readonly IConfiguration _configuration;

        public AuthController(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        [HttpPost]
        public ResponseModel<TokenModel> Login([FromBody] PasswordLoginModel model, bool isAdmin)
        {
            var ip = HttpContext.Connection.RemoteIpAddress!.ToString();
            var user = _authService.Login(model.LoginID!, model.Password!, isAdmin, ip);
            if (user.Item1 == Message.Success)
            {
                return new ResponseModel<TokenModel>(CreateToken(user.Item2!));
            }
            return new ResponseModel<TokenModel> { ErrorCode = user.Item1 };
        }

        [NonAction]
        private TokenModel CreateToken(Claim[] claims)
        {
            var authSection = _configuration.GetSection("AuthSettings");
            var expired = TimeSpan.FromSeconds(authSection["Expire"].ToInt());
            var now = DateTime.Now;
            var token = new JwtSecurityToken(
                issuer: authSection["Issuer"],
                audience: authSection["Audience"],
                claims: claims,
                notBefore: now,
                expires: now.Add(expired),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSection["IssuerSigningKey"]!)), SecurityAlgorithms.HmacSha256));
            var handler = new JwtSecurityTokenHandler();
            return new TokenModel { AccessToken = handler.WriteToken(token), ExpiresIn = (long)expired.TotalSeconds };
        }

        [HttpPost]
        public ResponseModel<TokenModel> LoginCodeSubmit([FromBody] VerifyCodeValidModel model)
        {
            var user = _authService.LoginCodeSubmit(model);
            if (user != null)
            {
                return new ResponseModel<TokenModel>(CreateToken(user));
            }
            return new ResponseModel<TokenModel> { ErrorCode = LoginCode.LoginFormInvalid };
        }

        [HttpPost]
        public ResponseModel<string> SendCodeLogin([FromBody] VerifyCodeValidModel model)
        {
            return new ResponseModel<string>(_authService.SendLoginCode(model));
        }

        [HttpPost]
        public ResponseModel<string> Register([FromBody] UserRegisterDto model)
        {
            return new ResponseModel<string>(_authService.Register(model));
        }

        [HttpPost]
        public ResponseModel<bool> SendCodeRegister(string phone)
        {
            return new ResponseModel<bool>(_authService.SendLoginRegister(phone));
        }
    }
}
