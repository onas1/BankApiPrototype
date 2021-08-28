using BankApi.Models;
using System;

namespace BankApi.Service.Abstractions
{
    public interface ITransactionService
    {
        Response CreateNewTransaction(Transaction transaction);
        Response FindTransactionByDate(DateTime date);
        Response MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin);
        Response MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin);
        Response MakeFundTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin);

    }
}
