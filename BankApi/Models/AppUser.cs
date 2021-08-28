using Microsoft.AspNetCore.Identity;

namespace BankApi.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }

        public Address Address { get; set; }


    }

    public enum Gender
    {
        Male,
        Female,
        Others
    }

    public class Address
    {
        public string StreetAddress { get; set; }
        public string streetAddress2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
