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
                .ForMember(x => x.UserId, opt => opt.Ignore())
                .ForMember(x => x.UserId, opt => opt.Ignore());
        }
    }
}
