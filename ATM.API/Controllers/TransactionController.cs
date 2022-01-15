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
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(ITransactionService transactionService, IMapper mapper, ILogger<TransactionController> logger)
        {
            _transactionService = transactionService;
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
        public IActionResult CreateTransaction(Transaction transaction)
        {
            Transaction newTransaction = _mapper.Map<Transaction>(transaction);
            newTransaction.Id = transaction.BankId.GenTransactionId(transaction.AccountId);
            _transactionService.AddTransaction(newTransaction);
            _logger.Log(LogLevel.Information, message: "New Transaction Created");
            return Created($"{Request.Path}/{transaction.AccountId}", newTransaction);
        }
    }
}
