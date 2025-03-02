using AutoMapper;
using Bank.Credits.Api.Models.Credits;
using Bank.Credits.Application.Credits.Models;

namespace Bank.Credits.Api.Mappers
{
    /// <summary>
    /// Настройка AutoMapper для API
    /// </summary>
    public class CreditApiMapperProfile : Profile
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public CreditApiMapperProfile()
        {
            CreateMap<TakeCreditRequest, TakeCreditDto>();
            CreateMap<ReduceCreditRequest, ReduceCreditDto>();
        }
    }
}
