using Microsoft.Extensions.Configuration;

namespace BankApi.Service.Abstractions
{
    public interface IJwtTokenGenerator
    {
        public string GenerateToken(string UserName, string UserId, string Email, IConfiguration Config);

    }
}
