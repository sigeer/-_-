using DDDDomain.Shared.Users;
using DDDEF;
using DDDEF.Models;
using DDDUtility;
using Utility.Extensions;

namespace DDDDomain.Users
{
    public class UserManager
    {
        readonly StorageDbContext _dbContext;

        public UserManager(StorageDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User RegisterUser(string? loginId, string? password, string? phoneNumber, int userType)
        {
            if (_dbContext.Users.Any(x => x.LoginId == loginId))
                throw new BusinessException(UserErrorCode.AccountNameDuplicate);

            if (_dbContext.Users.Any(x => x.PhoneNumber == phoneNumber))
                throw new BusinessException(UserErrorCode.PhoneDuplicate);

            var name = $"User_{6.GenerateRandomStr(false)}";
            while (_dbContext.Users.Any(x => x.Name == name))
            {
                name = $"User_{6.GenerateRandomStr(false)}";
            }
            //注册只能注册学生or教师
            return new User(loginId, password, phoneNumber, name, userType % 2 == 0 ? 2 : 1);
        }
    }
}
