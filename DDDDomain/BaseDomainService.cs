using AutoMapper;
using DDDDomain.Users;
using DDDUtility.Autofac;

namespace DDDDomain
{
    public class BaseDomainService
    {
        [Autowired]
        protected IIdentityUserContainer IdentityUser { get; set; } = null!;
        protected int UserId => IdentityUser.UserId;
        [Autowired]
        protected IMapper Mapper { get; set; } = null!;
    }
}
