using ATM.Services;
using ATM.Services.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ATM.Services.Exceptions;
using ATM.Models;

namespace ATM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : Controller
    {
        private readonly ICurrencyService _currencyService;
        private readonly IMapper _mapper;
        private readonly BankContext _bankContext;
        private readonly ILogger<CurrencyController> _logger;
        public CurrencyController(ICurrencyService currencyService, IMapper mapper, BankContext bankContext, ILogger<CurrencyController> logger)
        {
            _currencyService = currencyService;
            _mapper = mapper;
            _bankContext = bankContext;
            _logger = logger;
        }

        [HttpGet("{bankId}")]
        public IActionResult GetCurrencies(string bankId)
        {
            _logger.Log(LogLevel.Information, message: "Fetched All Currencies of a Bank");
            return Ok(_currencyService.GetAllCurrencies(bankId));
        }

        [HttpGet("{bankId}/{currencyName}")]
        public IActionResult GetCurrency(string bankId, string currencyName)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: "Fetched a Currency by name");
                return Ok(_currencyService.GetCurrencyByName(bankId, currencyName));
            }
            catch(CurrencyDoesNotExistException ex)
            {
                _logger.Log(LogLevel.Error, message: ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpPost("{bankId}")]
        public IActionResult CreateCurrency(string bankId, Currency currency)
        {
            _currencyService.AddCurrency(currency);
            _logger.Log(LogLevel.Information, message: "Currrency Created Successfully");
            return Created($"{Request.Path}/{currency.Name}", currency);
        }

        [HttpPut("{bankId}/{currencyName}")]
        public IActionResult UpdateCurrency(string bankId, string currencyName, Currency updatedCurrency)
        {
            try
            {
                _currencyService.UpdateCurrency(bankId, currencyName, updatedCurrency);
                _logger.Log(LogLevel.Information, message: "Currency Updated Successfully");
                return Ok(_currencyService.GetCurrencyByName(bankId, currencyName));
            }
            catch (CurrencyDoesNotExistException ex)
            {
                _logger.Log(LogLevel.Error, message: ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{bankId}/{currencyName}")]
        public IActionResult DeleteCurrency(string bankId, string currencyName)
        {
            try
            {
                _currencyService.DeleteCurrency(bankId, currencyName);
                _logger.Log(LogLevel.Information, message: "Currency deleted successsfully");
                return Ok("Currency deleted successsfully");
            }
            catch(CurrencyDoesNotExistException ex)
            {
                _logger.Log(LogLevel.Error, message: ex.Message);
                return NotFound(ex.Message);
            }
        }
    }
}
