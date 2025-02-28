using AutoMapper;
using Bank.Users.Application.Users.Models;
using Bank.Users.Domain.Users;

namespace Bank.Users.Application.Users.Mapper
{
    public class UserApplicationMapperProfile : Profile
    {
        public UserApplicationMapperProfile()
        {
            CreateMap<UserEntity, UserDto>();
        }
    }
}
