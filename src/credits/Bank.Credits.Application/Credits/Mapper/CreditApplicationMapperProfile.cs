using AutoMapper;
using Bank.Credits.Application.Credits.Models;
using Bank.Credits.Domain.Credits;

namespace Bank.Credits.Application.Credits.Mapper
{
    public class CreditApplicationMapperProfile : Profile
    {
        public CreditApplicationMapperProfile()
        {
            CreateMap<TakeCreditDto, Credit>()
                .ForMember(x => x.DebtAmount, opt => opt.MapFrom(x => x.LoanAmount))
                .ForMember(x => x.UserId, opt => opt.Ignore())
                .ForMember(x => x.UserId, opt => opt.Ignore());

            CreateMap<Credit, CreditShortDto>()
                 .ForMember(x => x.NextPaymentAmount, opt => opt.MapFrom(_ => 100M))
                 .ForMember(x => x.NextPaymentDateOnly, opt => opt.MapFrom(_ => new DateOnly(2025, 4, 4)));

            CreateMap<Credit, CreditDto>()
                .ForMember(x => x.NextPayments, opt => opt.MapFrom(_ => new List<NextPaymentDto>()))
                .ForMember(x => x.PaymentHistory, opt => opt.MapFrom(_ => new List<PaymentDto>()));  
        }
    }
}
