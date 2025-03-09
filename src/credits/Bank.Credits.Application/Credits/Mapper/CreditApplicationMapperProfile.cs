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
                .ForMember(x => x.DebtAmount, opt => opt.MapFrom(x => Math.Round(x.DebtAmount, 2)))
                .ForMember(x => x.NextPaymentAmount, opt => opt.MapFrom(_ => 0M))
                .ForMember(x => x.NextPaymentDateOnly, opt => opt.MapFrom(_ => new DateOnly(1, 1, 1)));

            CreateMap<Credit, CreditDto>()
                .ForMember(x => x.DebtAmount, opt => opt.MapFrom(x => Math.Round(x.DebtAmount, 2)))
                .ForMember(x => x.NextPayments, opt => opt.Ignore());

            CreateMap<Payment, PaymentDto>();
        }
    }
}
