using BankApi.DTOS;
using BankApi.Models;
using System.Threading.Tasks;

namespace BankApi.Service.Abstractions
{
    public interface IAuthService
    {
        public Task<Response> SignIn(LoginDTO model);
        public Task<Response> SignOut();
        public Task<Response> Register(AppUser model, string password);


    }
}
