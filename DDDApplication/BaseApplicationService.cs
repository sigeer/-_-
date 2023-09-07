using AutoMapper;
using DDDDomain.Users;
using DDDUtility.Autofac;

namespace DDDApplication
{
    public class BaseApplicationService
    {
        [Autowired]
        protected IIdentityUserContainer IdentityUser { get; set; } = null!;
        protected int UserId => IdentityUser.UserId;
        [Autowired]
        protected IMapper Mapper { get; set; } = null!;


    }
}
