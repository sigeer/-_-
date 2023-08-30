using AutoMapper;
using DDDDomain.Users;

namespace DDDDomain
{
    public class BaseDomainService
    {
        protected IIdentityUserContainer IdentityUser { get; set; } = null!;
        protected int UserId => IdentityUser.UserId;
        protected IMapper Mapper { get; set; } = null!;
    }
}
