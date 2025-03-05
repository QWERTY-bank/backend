using AutoMapper;
using Bank.Credits.Application.Tariffs.Models;
using Bank.Credits.Domain.Tariffs;

namespace Bank.Credits.Application.Tariffs.Mapper
{
    public class TariffApplicationMapperProfile : Profile
    {
        public TariffApplicationMapperProfile()
        {
            CreateMap<Tariff, TariffDto>();
            CreateMap<CreateTariffDto, Tariff>();
        }
    }
}
