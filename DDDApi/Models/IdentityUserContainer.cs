using DDDDomain.Users;
using Utility.Extensions;

namespace DDDApi.Models
{
    public class IdentityUserContainer : IIdentityUserContainer
    {
        public int UserId { get; set; }
        public IdentityUserContainer(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor?.HttpContext?.User?.Identity?.GetUserId() ?? 0;
        }
    }
}
