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
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly IEncryptionService _encryptionService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper, IEncryptionService encryptionService, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _logger = logger;
            _encryptionService = encryptionService;
        }

        [HttpGet("id/{id}")]
        public IActionResult GetEmployeeById(string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching Employee By id {id}");
                return Ok(_employeeService.GetEmployeeDetails(id));
            }
            catch (EmployeeDoesNotExistException ex)
            {
                _logger.Log(LogLevel.Error, message: ex.Message);
                return NotFound(ex.Message);
            }
        }
        [HttpGet("{bankId}/username/{username}")]
        public IActionResult GetEmployeeByUsername(string bankId, string username)
        {
            try
            {
                string employeeId = _employeeService.GetEmployeeIdByUsername(bankId, username);
                _logger.Log(LogLevel.Information, message: $"Fetching Employee with id {employeeId}");
                return Ok(_employeeService.GetEmployeeDetails(employeeId));
            }
            catch (EmployeeDoesNotExistException ex)
            {
                _logger.Log(LogLevel.Error, message: ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpPost("{bankId}")]
        public IActionResult CreateEmployee(string bankId, EmployeeCreateDTO employee)
        {
            Employee newEmployee = _mapper.Map<Employee>(employee);
            newEmployee.BankId = bankId;
            newEmployee.Id = employee.Name.GenId();
            (newEmployee.Password, newEmployee.Salt) = _encryptionService.ComputeHash(employee.Password);
            _employeeService.AddEmployee(newEmployee);
            _logger.Log(LogLevel.Information, message: "Created New Employee");
            return Created($"{Request.Path}/username/{newEmployee.Id}", _mapper.Map<EmployeeViewModel>(newEmployee));
        }

        [HttpPut("id/{id}")]
        public IActionResult UpdateEmployee(string id, EmployeeCreateDTO employee)
        {
            Employee newEmployee = _mapper.Map<Employee>(employee);
            (newEmployee.Password, newEmployee.Salt) = _encryptionService.ComputeHash(employee.Password);
            try
            {
                _employeeService.UpdateEmployee(id, newEmployee);
                _logger.Log(LogLevel.Information, message: "Employee Updated Succesfully");
                return Ok(_employeeService.GetEmployeeDetails(id));
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, message: ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("id/{id}")]
        public IActionResult DeleteEmployee(string id)
        {
            try
            {
                _employeeService.DeleteEmployee(id);
                _logger.Log(LogLevel.Information, message: "Employee Deleted Successfully");
                return Ok("Account Deleted Succesfully");
            }catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, message: ex.Message);
                return NotFound(ex.Message);
            }
        }
    }
}
