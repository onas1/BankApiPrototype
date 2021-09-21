using BankApi.DAL;
using BankApi.Models;
using BankApi.Service.Abstractions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApi.Service.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;

        public AccountService(ApplicationDbContext DbContext, UserManager<AppUser> userManager)
        {
            _dbContext = DbContext;
            _userManager = userManager;
        }



        public Account Authenticate(string AccountNumber, string pin)
        {
            //lets make authentication
            //check if account exit for account number
            var account = _dbContext.Accounts.SingleOrDefault(x => x.AccountNumberGenerated == AccountNumber);
            if (account == null)
                return null;

            //verify pinHash
            if (!VerifyPinHash(pin, account.PinHash, account.PinSalt))
                return null;

            //authentication passed
            return account;

        }

        public bool VerifyPinHash(string pin, byte[] pinHash, byte[] pintSalt)
        {
            if (string.IsNullOrWhiteSpace(pin)) throw new ArgumentNullException("pin");
            using (var hmac = new System.Security.Cryptography.HMACSHA512(pintSalt))
            {
                var computedPinHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pin));

                for (int i = 0; i < computedPinHash.Length; i++)
                {
                    if (computedPinHash[i] != pinHash[i]) return false;
                }
            }

            return true;
        }

        public IEnumerable<Account> GetAllAccounts(string userId)
        {
            return _dbContext.Accounts.Where(x => x.AppUser.Id == userId).ToList();
        }

        public async Task<Account> Create(Account account, string userId, string Pin, string ConfirmPin)
        {
            //this is to create a new account
            if (_dbContext.Accounts.Any(x => x.Email == account.Email))
                throw new ApplicationException("An Account already exit with this email");
            //validate pin
            if (!Pin.Equals(ConfirmPin))
                throw new ArgumentException("Pin do not match", "Pin");

            CreatePinHash(Pin, out var PinHash, out var pinSalt); //this create pin hash and salt.
            account.PinHash = PinHash;
            account.PinSalt = pinSalt;

            //add account to db
            var user = await _userManager.FindByIdAsync(userId);
            user.Account.Add(account);
            _dbContext.Accounts.Add((account));

            await _dbContext.SaveChangesAsync();

            return account;
        }

        private static void CreatePinHash(string pin, out byte[] pinHash, out byte[] pinSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                pinSalt = hmac.Key;
                pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
            }
        }


        public void Update(Account account, string pin = null)
        {
            //set to id and get id from route after setting up jwt authentication
            //and identity

            var acc = _dbContext.Accounts.FirstOrDefault(a => a.Id == account.Id);
            if (!string.IsNullOrWhiteSpace(account.Email))
            {

                if (_dbContext.Accounts.Any(x => x.Email == account.Email))
                    throw new ApplicationException("This Email" + account.Email + "already exist");
                acc.Email = account.Email;
            }


            if (!string.IsNullOrWhiteSpace(account.PhoneNumber))
            {

                if (_dbContext.Accounts.Any(x => x.PhoneNumber == account.PhoneNumber))
                    throw new ApplicationException("This Phone Number" + account.PhoneNumber + "already exist");
                acc.PhoneNumber = account.PhoneNumber;
            }


            if (!string.IsNullOrWhiteSpace(pin))
            {
                byte[] pinHash, pinSalt;
                CreatePinHash(pin, out pinHash, out pinSalt);

                acc.PinHash = pinHash;
                acc.PinSalt = pinSalt;
                acc.DateLastUpdated = account.DateLastUpdated;

            }
            //now persist this update

            _dbContext.Accounts.Update(acc);
            _dbContext.SaveChanges();




        }

        public void Delete(int id)
        {
            var account = _dbContext.Accounts.Find(id);
            if (account != null)
            {
                _dbContext.Accounts.Remove((account));
                _dbContext.SaveChanges();
            }
        }

        public Account GetById(int Id)
        {
            return _dbContext.Accounts.FirstOrDefault(a => a.Id == Id);
        }

        public Account GetByAccountNumber(string accountNumber)
        {
            return _dbContext.Accounts.FirstOrDefault(a => a.AccountNumberGenerated == accountNumber);
        }
    }
}
