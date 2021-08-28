using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApi.Models
{
    [Table("Transaction")]

    public class Transaction
    {
        public Transaction()
        {
            TransactionUniqueReference = $"{Guid.NewGuid().ToString().Replace("_", "").Substring(1, 27)}"; //using guid to generate this value.
        }

        [Key]
        public int Id { get; set; }
        public string TransactionUniqueReference { get; set; } // this will generate in every instance of this class
        public decimal TransactionAmount { get; set; }
        public TranStatus TransactionStatus { get; set; }
        public bool IsSuccessful => TransactionStatus.Equals(TranStatus.Success); //this depends on the value of the transaction status
        public string TransactionSourceAccount { get; set; }
        public DateTime TransactionDate { get; set; }

        public string TransactionDestinationAccount { get; set; }
        public string TransactionParticulars { get; set; }
        public TransType TransactionType { get; set; } //another enum

    }


    public enum TranStatus
    {
        Failed,
        Success,
        Error

    }

    public enum TransType
    {
        Deposit,
        Withdrawal,
        Transfer
    }


}
