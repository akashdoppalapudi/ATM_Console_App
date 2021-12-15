using ATM.Models;
using ATM.Models.Enums;
using ATM.Services.DBModels;
using ATM.Services.Exceptions;
using ATM.Services.IServices;
using System;
using System.Linq;

namespace ATM.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly IMapperService _mapperService;
        private readonly BankContext _bankContext;

        public EmployeeService(IEncryptionService encryptionService, BankContext bankContext, IMapperService mapperService)
        {
            _encryptionService = encryptionService;
            _mapperService = mapperService;
            _bankContext = bankContext;
        }

        private Employee GetEmployeeById(string bankId, string employeeId)
        {
            CheckEmployeeExistance(bankId, employeeId);
            EmployeeDBModel employeeRecord = _bankContext.Employee.FirstOrDefault(e => e.BankId == bankId && e.Id == employeeId && e.IsActive);
            return _mapperService.MapDBToEmployee(employeeRecord);
        }

        public void CheckEmployeeExistance(string bankId, string employeeId)
        {
            if (!_bankContext.Employee.Any(e => e.BankId == bankId && e.Id == employeeId && e.IsActive))
            {
                throw new EmployeeDoesNotExistException();
            }
        }

        public Employee CreateEmployee(string name, Gender gender, string username, string password, EmployeeType employeeType)
        {
            (byte[] passwordBytes, byte[] saltBytes) = _encryptionService.ComputeHash(password);
            return new Employee
            {
                Id = name.GenId(),
                Name = name,
                Gender = gender,
                Username = username,
                Password = passwordBytes,
                Salt = saltBytes,
                EmployeeType = employeeType
            };
        }

        public string GetEmployeeIdByUsername(string bankId, string username)
        {
            string id;
            EmployeeDBModel employeeRecord = _bankContext.Employee.FirstOrDefault(e => e.BankId == bankId && e.IsActive && e.Username == username);
            if (employeeRecord == null)
            {
                throw new EmployeeDoesNotExistException();
            }
            id = employeeRecord.Id;
            return id;
        }

        public void AddEmployee(string bankId, Employee employee)
        {
            employee.BankId = bankId;
            EmployeeDBModel employeeRecord = _mapperService.MapEmployeeToDB(employee);
            _bankContext.Employee.Add(employeeRecord);
            _bankContext.SaveChanges();
        }

        public void UpdateEmployee(string bankId, string employeeId, Employee UpdateEmployee)
        {
            Employee employee = GetEmployeeById(bankId, employeeId);
            employee.Name = UpdateEmployee.Name;
            employee.Gender = UpdateEmployee.Gender;
            employee.Username = UpdateEmployee.Username;
            if (UpdateEmployee.Password != _encryptionService.ComputeHash("", UpdateEmployee.Salt))
            {
                employee.Password = UpdateEmployee.Password;
                employee.Salt = UpdateEmployee.Salt;
            }
            employee.EmployeeType = UpdateEmployee.EmployeeType;
            EmployeeDBModel currentEmployeeRecord = _bankContext.Employee.First(e => e.BankId == employee.BankId && e.Id == employee.Id && e.IsActive);
            currentEmployeeRecord.Name = employee.Name;
            currentEmployeeRecord.Gender = employee.Gender;
            currentEmployeeRecord.Username = employee.Username;
            currentEmployeeRecord.Password = employee.Password;
            currentEmployeeRecord.Salt = employee.Salt;
            currentEmployeeRecord.EmployeeType = employee.EmployeeType;
            _bankContext.SaveChanges();
        }

        public void DeleteEmployee(string bankId, string employeeId)
        {
            CheckEmployeeExistance(bankId, employeeId);
            EmployeeDBModel employeeRecord = _bankContext.Employee.First(e => e.Id == employeeId && e.BankId == bankId && e.IsActive);
            employeeRecord.IsActive = false;
            employeeRecord.DeletedOn = DateTime.Now;
            _bankContext.SaveChanges();
        }

        public Employee GetEmployeeDetails(string bankId, string employeeId)
        {
            Employee employee = GetEmployeeById(bankId, employeeId);
            return new Employee
            {
                BankId = employee.BankId,
                Name = employee.Name,
                Gender = employee.Gender,
                Username = employee.Username,
                EmployeeType = employee.EmployeeType
            };
        }

        public bool IsEmployeeAdmin(string bankId, string employeeId)
        {
            Employee employee = GetEmployeeById(bankId, employeeId);
            return employee.EmployeeType == EmployeeType.Admin;
        }

        public void ValidateUsername(string bankId, string username)
        {
            if (_bankContext.Employee.Any(e => e.BankId == bankId && e.Username == username && e.IsActive))
            {
                throw new UsernameAlreadyExistsException();
            }
        }

        public void Authenticate(string bankId, string employeeId, string password)
        {
            Employee employee = GetEmployeeById(bankId, employeeId);
            if (Convert.ToBase64String(employee.Password) != Convert.ToBase64String(_encryptionService.ComputeHash(password, employee.Salt)))
            {
                throw new AuthenticationFailedException();
            }
        }
    }
}
