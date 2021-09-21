using BankApi.Models;

namespace BankApi.DTOS
{
    public class RegisterUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public Gender Gender { get; set; }
        public string Password { get; set; }
        public string AddressStreet { get; set; }
        public string AddressStreet2 { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string AddressCountry { get; set; }
    }

}
