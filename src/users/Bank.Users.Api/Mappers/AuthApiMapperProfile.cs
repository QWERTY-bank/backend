using AutoMapper;
using Bank.Users.Api.Models.Auth;
using Bank.Users.Application.Auth.Models;

namespace Bank.Users.Api.Mappers
{
    /// <summary>
    /// Настройка AutoMapper для API
    /// </summary>
    public class AuthApiMapperProfile : Profile
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public AuthApiMapperProfile()
        {
            CreateMap<RegistrationAuthRequest, RegistrationDTO>();
            CreateMap<LoginAuthRequest, LoginDTO>();
        }
    }
}
