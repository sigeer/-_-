using AutoMapper;
using DDDApplication.Contract.Auth;
using DDDApplication.Contract.Users;
using DDDDomain.Shared.Users;
using DDDEF.Models;

namespace DDDApplication.Users
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<AuthUserInfoModel, AuthUserInfoDto>();
            CreateMap<RoleBase, RoleInfoDto>();
        }
    }
}
