using DDDApplication.Contract.Auth;
using DDDApplication.Contract.Users;
using Utility.Constants;

namespace DDDApi.Controllers
{
    public class UserController: BaseApiController
    {
        readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public ResponseModel<AuthUserInfoDto?> GetUserInfo()
        {
            return new ResponseModel<AuthUserInfoDto?>(_userService.GetUserInfo(UserId));
        }
    }
}
