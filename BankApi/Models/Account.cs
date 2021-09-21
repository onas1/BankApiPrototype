using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BankApi.Models
{
    [Table("Accounts")]
    public class Account
    {
        private Random rand = new Random();
        public Account()
        {
            //AccountName = $"{FirstName} {LastName}";

            AccountNumberGenerated = Convert.ToString((long)Math.Floor((rand.NextDouble() * 9_000_000_000L + 1_000_000_000L))); // did 9_000_000_000 so we can have a ten digit random number
        }
        [Key]
        public int Id { get; set; }

        public string AppuserId { get; set; }

        public AppUser AppUser { get; set; }
        public List<Transaction> Transaction { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal CurrentAccountBalance { get; set; }
        public AccountType AccountType { get; set; } //enum for account type
        public string AccountNumberGenerated { get; set; } //generate account number here

        [JsonIgnore]
        public byte[] PinHash { get; set; } //to store hash and salt of account transaction pin

        [JsonIgnore]
        public byte[] PinSalt { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }
    }

    public enum AccountType
    {
        Savings,
        Current,
        Corportate,
        Goverment
    }
}
