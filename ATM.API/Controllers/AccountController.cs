using ATM.API.Models;
using ATM.Models;
using ATM.Models.ViewModels;
using ATM.Services.Exceptions;
using ATM.Services.Extensions;
using ATM.Services.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ATM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly IEncryptionService _encryptionService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, IMapper mapper, ILogger<AccountController> logger, IEncryptionService encryptionService)
        {
            _accountService = accountService;
            _mapper = mapper;
            _logger = logger;
            _encryptionService = encryptionService;
        }

        [HttpGet("id/{id}")]
        public IActionResult GetAccountById(string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching Account by Id {id}");
                return Ok(_accountService.GetAccountDetails(id));
            }
            catch (AccountDoesNotExistException ex)
            {
                _logger.Log(LogLevel.Error, message: ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{bankId}/username/{username}")]
        public IActionResult GetAccountByUsername(string bankId, string username)
        {
            try
            {
                string accountId = _accountService.GetAccountIdByUsername(bankId, username);
                _logger.Log(LogLevel.Information, message: $"Fetching Account by Id {accountId}");
                return Ok(_accountService.GetAccountDetails(accountId));
            }
            catch (AccountDoesNotExistException ex)
            {
                _logger.Log(LogLevel.Error, message: ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateAccount(AccountCreateDTO account)
        {
            Account newAccount = _mapper.Map<Account>(account);
            newAccount.Id = account.Name.GenId();
            (newAccount.Password, newAccount.Salt) = _encryptionService.ComputeHash(account.Password);
            _accountService.AddAccount(newAccount);
            _logger.Log(LogLevel.Information, message: "Created New Account");
            return Created($"{Request.Path}/id/{newAccount.Id}", _mapper.Map<AccountViewModel>(newAccount));
        }

        [HttpPut("id/{id}")]
        public IActionResult UpdateAccount(string id, AccountCreateDTO account)
        {
            Account newAccount = _mapper.Map<Account>(account);
            (newAccount.Password, newAccount.Salt) = _encryptionService.ComputeHash(account.Password);
            try
            {
                _accountService.UpdateAccount(id, newAccount);
                _logger.Log(LogLevel.Information, message: "Account Updated Succesfully");
                return Ok(_accountService.GetAccountDetails(id));
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, message: ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("id/{id}")]
        public IActionResult DeleteAccount(string id)
        {
            try
            {
                _accountService.DeleteAccount(id);
                _logger.Log(LogLevel.Information, message: "Deleted account successfully");
                return Ok("Account Deleted Succesfully");
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, message: ex.Message);
                return NotFound(ex.Message);
            }
        }
    }
}
