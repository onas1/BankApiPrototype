using BankApi.Models;
using System.Collections.Generic;

namespace BankApi.Service.Abstractions
{
    public interface IAccountService
    {
        Account Authenticate(string AccountNumber, string pin);
        IEnumerable<Account> GetAllAccounts();
        Account Create(Account account, string Pin, string ConfirmPin);
        void Update(Account account, string pin = null);
        void Delete(int Id);
        Account GetById(int Id);
        Account GetByAccountNumber(string AccountNumber);
    }
}
