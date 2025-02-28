using AutoMapper;
using Bank.Users.Api.Models.Profile;
using Bank.Users.Application.Users.Models;

namespace Bank.Users.Api.Mappers
{
    /// <summary>
    /// Профиль маппинга для API
    /// </summary>
    public class UserApiMapperProfile : Profile
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public UserApiMapperProfile()
        {
            CreateMap<ChangePhoneProfileRequest, ChangePhoneDto>();
            CreateMap<ChangePasswordProfileRequest, ChangePasswordDto>();
            CreateMap<ChangeProfileRequest, ChangeProfileDto>();
        }
    }
}
