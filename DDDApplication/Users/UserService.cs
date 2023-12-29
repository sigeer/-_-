using DDDApplication.Contract.Auth;
using DDDApplication.Contract.Users;
using DDDDomain.Users;
using DDDEF;

namespace DDDApplication.Users
{
    public class UserService : BaseApplicationService, IUserService
    {
        readonly StorageDbContext _dbContext;
        readonly UserManager _userManager;

        public UserService(StorageDbContext dbContext, UserManager userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public AuthUserInfoDto? GetUserInfo(int userId)
        {
            var user = _userManager.GetUser(userId);
            if (user != null)
            {
                var model = Mapper.Map<AuthUserInfoDto>(user);
                model.Roles = Mapper.Map<List<RoleInfoDto>>(_userManager.GetUserRoleList(userId));
                return model;
            }
            return null;
        }
    }
}
