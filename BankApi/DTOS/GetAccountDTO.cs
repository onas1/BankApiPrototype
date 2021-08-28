using BankApi.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BankApi.DTOS
{
    public class GetAccountDTO
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal CurrentAccountBalance { get; set; }
        public AccountType AccountType { get; set; } //enum for account type
        public string AccountNumberGenerated { get; set; } //generate account number here
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }
    }
}

