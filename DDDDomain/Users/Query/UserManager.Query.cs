using DDDDomain.Shared.Users;
using DDDEF.Models;
using Microsoft.EntityFrameworkCore;

namespace DDDDomain.Users
{
    public partial class UserManager
    {
        public List<RoleBase> GetUserRoleList(int userId)
        {
            return (from a in _dbContext.UserRoles
                    join b in _dbContext.RoleBase on a.RoleId equals b.Id
                    where a.UserId == userId
                    select b).AsNoTracking().ToList();
        }
        public AuthUserInfoModel? GetUser(int userid)
        {
            return _dbContext.Users.Where(u => u.Id == userid).Select(x => new AuthUserInfoModel
            {
                Id = x.Id,
                Avatar = x.Avatar,
                Email = x.Email,
                Name = x.Name,
                Type = x.Type
            }).FirstOrDefault();
        }
    }
}
