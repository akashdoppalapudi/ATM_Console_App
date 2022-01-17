using ATM.API.Models;
using ATM.Models;
using ATM.Services.Exceptions;
using ATM.Services.Extensions;
using ATM.Services.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ATM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankController : Controller
    {
        private readonly IBankService _bankService;
        private readonly ILogger<BankController> _logger;
        public readonly IMapper _mapper;
        public BankController(IBankService bankService, ILogger<BankController> logger, IMapper mapper)
        {
            _bankService = bankService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetBanks()
        {
            try
            {
                _logger.Log(LogLevel.Information, message: "Fetching all the banks");
                return Ok(_mapper.Map<BankDTO[]>(_bankService.GetAllBanks()));
            }
            catch (NoBanksException ex)
            {
                _logger.Log(LogLevel.Error, message: ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{bankId}")]
        public IActionResult GetBank(string bankId)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching the bank with the id {bankId}");
                return Ok(_mapper.Map<BankDTO>(_bankService.GetBankDetails(bankId)));
            }
            catch (BankDoesnotExistException ex)
            {
                _logger.Log(LogLevel.Error, message: ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateBank(Bank bank)
        {
            Bank newBank = new Bank
            {
                Name = bank.Name,
                Id = bank.Name.GenId()
            };
            _bankService.AddBank(newBank);
            _logger.Log(LogLevel.Information, message: "Created a Bank");
            return Created($"{Request.Path}/{newBank.Id}", _mapper.Map<BankDTO>(newBank));
        }

        [HttpPut("{bankId}")]
        public IActionResult UpdateBank(string bankId, Bank updatedBank)
        {
            try
            {
                _bankService.UpdateBank(bankId, updatedBank);
                _logger.Log(LogLevel.Information, message: "Bank Updated Sucessfully");
                return Ok(_bankService.GetBankDetails(bankId));
            }
            catch (BankDoesnotExistException ex)
            {
                _logger.Log(LogLevel.Error, message: ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{bankId}")]
        public IActionResult DeleteBank(string bankId)
        {
            try
            {
                _bankService.DeleteBank(bankId);
                _logger.Log(LogLevel.Information, message: "Bank Deleted Sucessfully");
                return Ok("Bank Deleted Sucessfully");
            }
            catch (BankDoesnotExistException ex)
            {
                _logger.Log(LogLevel.Error, message: ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpPost("isbanknameexists")]
        public IActionResult IsBankNameExists(BankNameDTO bankName)
        {
            return Ok(new { BankNameExists = _bankService.IsBankNameExists(bankName.BankName) });
        }
    }
}
