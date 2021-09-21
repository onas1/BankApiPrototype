using AutoMapper;
using BankApi.DTOS;
using BankApi.Models;

namespace BankApi.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterNewAccountDTo, Account>();
            CreateMap<UpdateAccountDTO, Account>();
            CreateMap<Account, GetAccountDTO>();
            CreateMap<AppUser, RegisterUserDTO>()
                .ForMember(dest => dest.AddressStreet, act => act.MapFrom(src => src.Address.StreetAddress))
                .ForMember(dest => dest.AddressStreet2, act => act.MapFrom(src => src.Address.StreetAddress2))
                .ForMember(dest => dest.AddressCity, act => act.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.AddressState, act => act.MapFrom(src => src.Address.State))
                .ForMember(dest => dest.AddressCountry, act => act.MapFrom(src => src.Address.Country)).ReverseMap();

            CreateMap<TransactionRequestDTO, Transaction>();
            //CreateMap<Address, AddressDTO>();

        }
    }
}
