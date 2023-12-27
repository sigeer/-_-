using DDDApplication.Contract.Auth;
using DDDApplication.Contract.Users;
using Utility.Constants;

namespace DDDApi.Controllers
{
    public class UserController: BaseApiController
    {
        readonly IUserService _userService;
        readonly IAuthService _authService;

        public UserController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        public ResponseModel<AuthUserInfoDto?> GetUserInfo()
        {
            return new ResponseModel<AuthUserInfoDto?>(_authService.GetUserInfo(UserId));
        }
    }
}
