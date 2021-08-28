using BankApi.Models;
using System;

namespace BankApi.DTOS
{
    public class TransactionRequestDTO
    {
        public decimal TransactionAmount { get; set; }
        public string TransactionSourceAccount { get; set; }
        public DateTime TransactionDate { get; set; }

        public string TransactionDestinationAccount { get; set; }
        public TransType TransactionType { get; set; } //another
    }
}
