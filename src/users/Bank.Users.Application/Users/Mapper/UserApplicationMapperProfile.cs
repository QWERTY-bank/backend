using AutoMapper;
using Bank.Users.Application.Users.Models;
using Bank.Users.Domain.Users;

namespace Bank.Users.Application.Users.Mapper
{
    public class UserApplicationMapperProfile : Profile
    {
        public UserApplicationMapperProfile()
        {
            CreateMap<UserEntity, UserDto>()
                .ForMember(x => x.MaxRole, opt => opt.MapFrom(y => y.Roles.Max(z => z.Type)));
            CreateMap<UserEntity, UserShortDto>()
                .ForMember(x => x.MaxRole, opt => opt.MapFrom(y => y.Roles.Max(z => z.Type)));
        }
    }
}
