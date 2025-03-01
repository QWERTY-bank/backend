using AutoMapper;
using Bank.Credits.Api.Models.Tariffs;
using Bank.Credits.Application.Tariffs.Models;

namespace Bank.Credits.Api.Mappers
{
    /// <summary>
    /// Настройка AutoMapper для API
    /// </summary>
    public class TariffApiMapperProfile : Profile
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public TariffApiMapperProfile()
        {
            CreateMap<CreateTariffRequest, CreateTariffDto>();
        }
    }
}
