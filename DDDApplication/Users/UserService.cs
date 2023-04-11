using DDDApplication.Contract.Users;
using DDDEF;

namespace DDDApplication.Users
{
    public class UserService : IUserService
    {
        readonly StorageDbContext _dbContext;

        public UserService(StorageDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
