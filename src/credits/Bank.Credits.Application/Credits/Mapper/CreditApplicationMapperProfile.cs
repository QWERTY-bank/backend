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
                .ForMember(x => x.PaymentsInfo.DebtAmount, opt => opt.MapFrom(x => x.LoanAmount))
                .ForMember(x => x.PaymentsInfo.PeriodDays, opt => opt.MapFrom(x => x.PeriodDays))
                .ForMember(x => x.UserId, opt => opt.Ignore());

            CreateMap<Credit, CreditShortDto>()
                .ForMember(x => x.DebtAmount, opt => opt.MapFrom(x => Math.Round(x.PaymentsInfo.DebtAmount, 2)))
                .ForMember(x => x.NextPaymentAmount, opt => opt.MapFrom(x => x.PaymentsInfo.NextPayment))
                .ForMember(x => x.NextPaymentDateOnly, opt => opt.MapFrom(x => x.PaymentsInfo.NextPaymentDate));

            CreateMap<Credit, CreditDto>()
                .ForMember(x => x.DebtAmount, opt => opt.MapFrom(x => Math.Round(x.PaymentsInfo.DebtAmount, 2)))
                .ForMember(x => x.NextPayments, opt => opt.MapFrom(src => src.NextRepayments()));

            CreateMap<Payment, PaymentDto>()
                .ForMember(x => x.PaymentType, opt => opt.MapFrom(src => src.Type));
            CreateMap<Payment, NextPaymentDto>()
                .ForMember(x => x.PaymentDateOnly, opt => opt.MapFrom(src => src.PaymentDate));
        }
    }
}
