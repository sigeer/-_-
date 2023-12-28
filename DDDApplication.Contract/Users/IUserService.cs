using DDDApplication.Contract.Auth;

namespace DDDApplication.Contract.Users
{
    public interface IUserService
    {
        AuthUserInfoDto? GetUserInfo(int userid);
    }
}
