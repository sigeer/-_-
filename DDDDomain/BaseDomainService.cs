using DDDDomain.Users;

namespace DDDDomain
{
    public class BaseDomainService
    {
        protected IIdentityUserContainer IdentityUser { get; set; } = null!;
        protected int UserId => IdentityUser.UserId;
    }
}
