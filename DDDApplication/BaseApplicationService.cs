using AutoMapper;
using DDDDomain.Users;

namespace DDDApplication
{
    public class BaseApplicationService
    {
        protected IIdentityUserContainer IdentityUser { get; set; } = null!;
        protected int UserId => IdentityUser.UserId;
        protected IMapper Mapper { get; set; } = null!;
    }
}
