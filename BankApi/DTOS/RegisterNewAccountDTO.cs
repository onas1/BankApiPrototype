using BankApi.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BankApi.DTOS
{
    public class RegisterNewAccountDTo
    {
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        public string AccountName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        //public decimal CurrentAccountBalance { get; set; }
        public AccountType AccountType { get; set; } //enum for account type
        //public string AccountNumberGenerated { get; set; } //generate account number here

        //public byte[] PinHash { get; set; } //to store hash and salt of account transaction pin
        //public byte[] PinSalt { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateLastUpdated { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Pin must not be more than four digits.")]
        public string Pin { get; set; }
        [Required]
        [Compare("Pin", ErrorMessage = "Pin does not match.")]
        public string ConfirmPin { get; set; }
    }
}
