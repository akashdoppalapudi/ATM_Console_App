using ATM.API.Models;
using ATM.Models;
using ATM.Services.Extensions;
using ATM.Services.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ATM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly IBankService _bankService;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(ITransactionService transactionService, IBankService bankService, IMapper mapper, ILogger<TransactionController> logger)
        {
            _transactionService = transactionService;
            _bankService = bankService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{accountId}")]
        public IActionResult GetAllTransactions(string accountId)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching all transactions of account {accountId}");
                return Ok(_transactionService.GetTransactions(accountId));
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, message: ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateTransaction(TransactionCreateDTO transaction)
        {
            Transaction newTransaction = _mapper.Map<Transaction>(transaction);
            newTransaction.Id = transaction.BankId.GenTransactionId(transaction.AccountId);
            _transactionService.AddTransaction(newTransaction);
            _logger.Log(LogLevel.Information, message: "New Transaction Created");
            return Created($"{Request.Path}/{transaction.AccountId}", newTransaction);
        }

        [HttpPost("revert/{id}")]
        public IActionResult RevertTransaction(string id)
        {
            try
            {
                _bankService.RevertTransaction(id);
                _logger.Log(LogLevel.Information, message: "Transaction reverted Successfully");
                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, message: ex.Message);
                return NotFound(ex.Message);
            }
        }
    }
}
