using AutoMapper;
using Bank.Credits.Application.User.Models;
using Bank.Credits.Domain.User;

namespace Bank.Credits.Application.User.Mapper
{
    public class UserApplicationMapperProfile : Profile
    {
        public UserApplicationMapperProfile()
        {
            CreateMap<UserEntity, UserCreditInfoDto>()
                .ForMember(x => x.Rating, opt => opt.MapFrom(src => 75))
                .ForMember(x => x.MaxRating, opt => opt.MapFrom(src => 100));
        }
    }
}
