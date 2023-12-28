using AutoMapper;
using DDDApplication.Contract.Users;
using DDDEF.Models;

namespace DDDApplication.Users
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<RoleBase, RoleInfoDto>();
        }
    }
}
