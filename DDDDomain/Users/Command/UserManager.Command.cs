using DDDDomain.Shared.Users;
using DDDEF.Models;
using DDDUtility;
using Utility.Extensions;

namespace DDDDomain.Users
{
    public partial class UserManager
    {
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

            return new User(loginId, password, phoneNumber, name, userType % 2 == 0 ? 2 : 1);
        }
    }
}
