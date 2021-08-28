using AutoMapper;
using BankApi.DTOS;
using BankApi.Models;
using BankApi.Service.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BankApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("/reigister_new_account")]
        public IActionResult RegisterNewAccount([FromBody] RegisterNewAccountDTo model)
        {
            if (!ModelState.IsValid) return BadRequest(model);
            var account = _mapper.Map<Account>(model);
            return Ok(_accountService.Create(account, model.Pin, model.ConfirmPin));
        }

        [HttpGet]
        [Route("get_all_account")]
        public IActionResult GetAllAccount()
        {
            var accounts = _accountService.GetAllAccounts();
            var cleanedAccount = _mapper.Map<IList<GetAccountDTO>>(accounts);
            return Ok(cleanedAccount);
        }

        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateDTO model)
        {
            if (!ModelState.IsValid) return BadRequest(model);
            //now lets map
            return Ok(_accountService.Authenticate(model.AccountNumber, model.Pin));
            //it returns an account
        }

        [HttpGet]
        [Route("get_by_account_number")]
        public IActionResult GetByAccountNumber(string AccountNumber)
        {
            if (Regex.IsMatch(AccountNumber, @"^[0][1-9]\d{9}$|^[1-9]{9}$")) return BadRequest("Account number must be 10-digit");
            var account = _accountService.GetByAccountNumber((AccountNumber));
            var cleanedAccount = _mapper.Map<GetAccountDTO>(account);
            return Ok(cleanedAccount);
        }


        [HttpGet]
        [Route("get_by_account_id")]
        public IActionResult GetAccountById(int Id)
        {
            var account = _accountService.GetById((Id));
            var cleanedAccount = _mapper.Map<GetAccountDTO>(account);
            return Ok(cleanedAccount);
        }

        [HttpPost]
        [Route("update_account")]
        public IActionResult UpdateAccount([FromBody] UpdateAccountDTO model)
        {
            if (!ModelState.IsValid) return BadRequest(model);
            var account = _mapper.Map<Account>(model);
            _accountService.Update(account, model.Pin);
            return Ok();

        }
    }
}
