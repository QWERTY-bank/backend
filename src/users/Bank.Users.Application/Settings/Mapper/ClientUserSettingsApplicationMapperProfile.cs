using AutoMapper;
using Bank.Users.Application.Settings.Models;
using Bank.Users.Domain.Settings;

namespace Bank.Users.Application.Settings.Mapper
{
    public class ClientUserSettingsApplicationMapperProfile : Profile
    {
        public ClientUserSettingsApplicationMapperProfile()
        {
            CreateMap<ClientUserSettings, ClientUserSettingsDto>();
        }
    }
}
