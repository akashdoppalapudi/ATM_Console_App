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
    public class EmployeeActionController : Controller
    {
        private readonly IEmployeeActionService _employeeActionService;
        private readonly ILogger<EmployeeActionController> _logger;
        private readonly IMapper _mapper;

        public EmployeeActionController(IEmployeeActionService employeeActionService, ILogger<EmployeeActionController> logger, IMapper mapper)
        {
            _employeeActionService = employeeActionService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("{employeeId}")]
        public IActionResult GetAllEmployeeActions(string employeeId)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching all employee actions of Employee {employeeId}");
                return Ok(_employeeActionService.GetEmployeeActions(employeeId));
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, message: ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateEmployeeAction(EmployeeActionCreateDTO employeeAction)
        {
            EmployeeAction newEmployeeAction = _mapper.Map<EmployeeAction>(employeeAction);
            newEmployeeAction.Id = employeeAction.BankId.GenEmployeeActionId(employeeAction.EmployeeId);
            _employeeActionService.AddEmployeeAction(newEmployeeAction);
            _logger.Log(LogLevel.Information, message: "New Action Created");
            return Created($"{Request.Path}/{newEmployeeAction.EmployeeId}", newEmployeeAction);
        }
    }
}
