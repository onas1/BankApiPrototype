using AutoMapper;
using BankApi.DTOS;
using BankApi.Models;
using BankApi.Service.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace BankApi.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public TransactionController(ITransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }


        //create new transaction
        [HttpPost]
        [Route("create_new_transaction")]
        public IActionResult CreateNewTransaction(TransactionRequestDTO model)
        {
            if (!ModelState.IsValid) return BadRequest(model);
            var transaction = _mapper.Map<Transaction>(model);
            return Ok(_transactionService.CreateNewTransaction(transaction));
        }


        [HttpPost]
        [Route("make_deposit")]
        public IActionResult MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {
            if (Regex.IsMatch(AccountNumber, @"^[0][1-9]\d{9}$|^[1-9]{9}$")) return BadRequest("Account number must be 10-digit");
            return Ok(_transactionService.MakeDeposit(AccountNumber, Amount, TransactionPin));
        }


        [HttpPost]
        [Route("make_withdrawal")]
        public IActionResult MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            if (Regex.IsMatch(AccountNumber, @"^[0][1-9]\d{9}$|^[1-9]{9}$")) return BadRequest("Account number must be 10-digit");
            return Ok(_transactionService.MakeWithdrawal(AccountNumber, Amount, TransactionPin));
        }


        [HttpPost]
        [Route("make_fund_transfer")]
        public IActionResult MakeFundTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {
            if (Regex.IsMatch(FromAccount, @"^[0][1-9]\d{9}$|^[1-9]{9}$") || Regex.IsMatch(ToAccount, @"^[0][1-9]\d{9}$|^[1-9]{9}$")) return BadRequest("Account number must be 10-digit");
            return Ok(_transactionService.MakeFundTransfer(FromAccount, ToAccount, Amount, TransactionPin));
        }

    }
}
