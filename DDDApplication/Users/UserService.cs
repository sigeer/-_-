using DDDApplication.Contract.Auth;
using DDDApplication.Contract.Users;
using DDDEF;

namespace DDDApplication.Users
{
    public class UserService : BaseApplicationService, IUserService
    {
        readonly StorageDbContext _dbContext;

        public UserService(StorageDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public AuthUserInfoDto? GetUserInfo(int userid)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userid);
            if (user != null)
            {
                var roleInfos = (from a in _dbContext.UserRoles
                                 join b in _dbContext.RoleBase on a.RoleId equals b.Id
                                 where a.UserId == userid
                                 select b).ToList();
                var model = new AuthUserInfoDto()
                {
                    Id = user.Id,
                    Avatar = user.Avatar,
                    Name = user.Name,
                    Email = user.Email,
                    Type = user.Type,
                    Roles = Mapper.Map<List<RoleInfoDto>>(roleInfos)
                };
                return model;
            }
            return null;
        }
    }
}
