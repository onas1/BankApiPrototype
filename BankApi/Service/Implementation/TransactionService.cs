using BankApi.DAL;
using BankApi.Models;
using BankApi.Service.Abstractions;
using BankApi.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace BankApi.Service.Implementation
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<TransactionService> _logger;
        private readonly AppSettings _settings;
        private static string _OurBankSettlementAccount;
        private readonly IAccountService _accountService;

        public TransactionService(ApplicationDbContext dbContext, ILogger<TransactionService> logger, IOptions<AppSettings> settings, IAccountService accountService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _accountService = accountService;
            _settings = settings.Value;
            _OurBankSettlementAccount = _settings.OurBankSettlementAccount;
        }
        public Response CreateNewTransaction(Transaction transaction)
        {
            var response = new Response();
            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction created successfully";
            return response;
        }

        public Response FindTransactionByDate(DateTime date)
        {
            var response = new Response();
            var transaction = _dbContext.Transactions.FirstOrDefault(tr => tr.TransactionDate == date);
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction created successfully";
            return response;
        }

        public Response MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {
            //make deposit
            var response = new Response();
            Account sourceAccount;
            Account DestinationAccount;
            Transaction transaction = new Transaction();

            //first check that user account owner is valid,
            //we will need authenticatethe user
            var authUser = _accountService.Authenticate(AccountNumber, TransactionPin);
            if (authUser == null) throw new ApplicationException("Invalid Credentials");

            //so validation passes
            try
            {
                //for deposit, our bankSettlementAccount is the source giving the money to the users account.
                sourceAccount = _accountService.GetByAccountNumber(_OurBankSettlementAccount);
                DestinationAccount = _accountService.GetByAccountNumber(AccountNumber);

                // now lets update their account balances
                sourceAccount.CurrentAccountBalance -= Amount;
                DestinationAccount.CurrentAccountBalance += Amount;
                //check if there is update
                if ((_dbContext.Entry(sourceAccount).State == EntityState.Modified) &&
                    (_dbContext.Entry(DestinationAccount).State == EntityState.Modified))
                {
                    //so transaction is successful
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction Successful";
                }
                else
                {
                    // so transaction uncessful
                    transaction.TransactionStatus = TranStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction failed!";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"AN ERROR OCCURRED... => {ex.Message}");
            }
            // set other props of transaction here
            transaction.TransactionType = TransType.Deposit;
            transaction.TransactionSourceAccount = _OurBankSettlementAccount;
            transaction.TransactionDestinationAccount = AccountNumber;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"NEW TRANSACTION FROM SOURCE " +
                                                 $"{JsonConvert.SerializeObject(transaction.TransactionSourceAccount)}" +
                                                 $" TO DESTINATION ACCOUNT =>" +
                                                 $" {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)}" +
                                                 $" ON DATE => {transaction.TransactionDate}" +
                                                 $" FOR AMOUNT => {JsonConvert.SerializeObject(transaction.TransactionAmount)}" +
                                                 $" TRANSACTION TYPE => {transaction.TransactionType}" +
                                                 $" TRANSACTION STATUS => {transaction.TransactionStatus}";
            //now lets save to db
            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();
            return response;


        }

        public Response MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            //make withdrawal
            var response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            //first chect that user account owner is valid,
            //we will need authenticatethe user
            var authUser = _accountService.Authenticate(AccountNumber, TransactionPin);
            if (authUser == null) throw new ApplicationException("Invalid Credentials");

            //so validation passes
            try
            {
                //for withdrawal, our bankSettlementAccount is the destination getting money from user's account.
                sourceAccount = _accountService.GetByAccountNumber(AccountNumber);
                destinationAccount = _accountService.GetByAccountNumber(_OurBankSettlementAccount);

                // now lets update their account balances
                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;
                //check if there is update
                if ((_dbContext.Entry(sourceAccount).State == EntityState.Modified) &&
                    (_dbContext.Entry(destinationAccount).State == EntityState.Modified))
                {
                    //so transaction is successful
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction Successful";
                }
                else
                {
                    // so transaction unsuccessful
                    transaction.TransactionStatus = TranStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction failed!";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"AN ERROR OCCURRED... => {ex.Message}");
            }
            // set other props of transaction here
            transaction.TransactionType = TransType.Withdrawal;
            transaction.TransactionSourceAccount = AccountNumber;
            transaction.TransactionDestinationAccount = _OurBankSettlementAccount;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"NEW TRANSACTION FROM SOURCE " +
                                                 $"{JsonConvert.SerializeObject(transaction.TransactionSourceAccount)}" +
                                                 $" TO DESTINATION ACCOUNT =>" +
                                                 $" {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)}" +
                                                 $" ON DATE => {transaction.TransactionDate}" +
                                                 $" FOR AMOUNT => {JsonConvert.SerializeObject(transaction.TransactionAmount)}" +
                                                 $" TRANSACTION TYPE => {transaction.TransactionType}" +
                                                 $" TRANSACTION STATUS => {transaction.TransactionStatus}";
            //now lets save to db
            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();
            return response;

        }

        public Response MakeFundTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {
            //make deposit
            var response = new Response();
            Account sourceAccount;
            Account DestinationAccount;
            Transaction transaction = new Transaction();

            //first chect that user account owner is valid,
            //we will need authenticatethe user
            var authUser = _accountService.Authenticate(FromAccount, TransactionPin);
            if (authUser == null) throw new ApplicationException("Invalid Credentials");

            //so validation passes
            try
            {
                //for deposit, our bankSettlementAccount is the source giving the money to the users account.
                sourceAccount = _accountService.GetByAccountNumber(FromAccount);
                DestinationAccount = _accountService.GetByAccountNumber(ToAccount);

                // now lets update their account balances
                sourceAccount.CurrentAccountBalance -= Amount;
                DestinationAccount.CurrentAccountBalance += Amount;
                //check if there is update
                if ((_dbContext.Entry(sourceAccount).State == EntityState.Modified) &&
                    (_dbContext.Entry(DestinationAccount).State == EntityState.Modified))
                {
                    //so transaction is successful
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction Successful";
                }
                else
                {
                    // so transaction uncessful
                    transaction.TransactionStatus = TranStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction failed!";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"AN ERROR OCCURRED... => {ex.Message}");
            }
            // set other props of transaction here
            transaction.TransactionType = TransType.Transfer;
            transaction.TransactionSourceAccount = FromAccount;
            transaction.TransactionDestinationAccount = ToAccount;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"NEW TRANSACTION FROM SOURCE " +
                                                 $"{JsonConvert.SerializeObject(transaction.TransactionSourceAccount)}" +
                                                 $" TO DESTINATION ACCOUNT =>" +
                                                 $" {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)}" +
                                                 $" ON DATE => {transaction.TransactionDate}" +
                                                 $" FOR AMOUNT => {JsonConvert.SerializeObject(transaction.TransactionAmount)}" +
                                                 $" TRANSACTION TYPE => {JsonConvert.SerializeObject(transaction.TransactionType)}" +
                                                 $" TRANSACTION STATUS => {transaction.TransactionStatus}";
            //now lets save to db
            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();
            return response;
        }
    }
}
