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
            CreateMap<TransactionRequestDTO, Transaction>();

        }
    }
}
