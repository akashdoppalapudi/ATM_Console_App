using ATM.Models;
using ATM.Models.Enums;
using ATM.Models.ViewModels;
using ATM.Services.DBModels;
using ATM.Services.Exceptions;
using ATM.Services.IServices;
using AutoMapper;
using System;
using System.Linq;

namespace ATM.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly IMapper _mapper;
        private readonly BankContext _bankContext;

        public EmployeeService(IEncryptionService encryptionService, BankContext bankContext, IMapper mapper)
        {
            _encryptionService = encryptionService;
            _mapper = mapper;
            _bankContext = bankContext;
        }

        public string GetEmployeeIdByUsername(string bankId, string username)
        {
            EmployeeDBModel employeeRecord = _bankContext.Employee.FirstOrDefault(e => e.BankId == bankId && e.IsActive && e.Username == username);
            if (employeeRecord == null)
            {
                throw new EmployeeDoesNotExistException();
            }
            return employeeRecord.Id;
        }

        public void AddEmployee(Employee employee)
        {
            EmployeeDBModel employeeRecord = _mapper.Map<EmployeeDBModel>(employee);
            _bankContext.Employee.Add(employeeRecord);
            _bankContext.SaveChanges();
        }

        public void UpdateEmployee(string employeeId, Employee updateEmployee)
        {
            EmployeeDBModel currentEmployeeRecord = _bankContext.Employee.FirstOrDefault(e => e.Id == employeeId && e.IsActive);
            if (currentEmployeeRecord == null)
            {
                throw new EmployeeDoesNotExistException();
            }
            currentEmployeeRecord.Name = updateEmployee.Name;
            currentEmployeeRecord.Gender = updateEmployee.Gender;
            currentEmployeeRecord.Username = updateEmployee.Username;
            if (Convert.ToBase64String(_encryptionService.ComputeHash("", updateEmployee.Salt)) != Convert.ToBase64String(updateEmployee.Password))
            {
                currentEmployeeRecord.Password = updateEmployee.Password;
                currentEmployeeRecord.Salt = updateEmployee.Salt;
            }
            currentEmployeeRecord.EmployeeType = updateEmployee.EmployeeType;
            _bankContext.SaveChanges();
        }

        public void DeleteEmployee(string employeeId)
        {
            EmployeeDBModel employeeRecord = _bankContext.Employee.FirstOrDefault(e => e.Id == employeeId && e.IsActive);
            if (employeeRecord == null)
            {
                throw new EmployeeDoesNotExistException();
            }
            employeeRecord.IsActive = false;
            employeeRecord.DeletedOn = DateTime.Now;
            _bankContext.SaveChanges();
        }

        public EmployeeViewModel GetEmployeeDetails(string employeeId)
        {
            EmployeeDBModel employeeRecord = _bankContext.Employee.FirstOrDefault(e => e.Id == employeeId && e.IsActive);
            if (employeeRecord == null)
            {
                throw new EmployeeDoesNotExistException();
            }
            return _mapper.Map<EmployeeViewModel>(employeeRecord);
        }

        public bool IsEmployeeAdmin(string employeeId)
        {
            EmployeeDBModel employeeRecord = _bankContext.Employee.FirstOrDefault(e => e.Id == employeeId && e.IsActive);
            if (employeeRecord == null)
            {
                throw new EmployeeDoesNotExistException();
            }
            return employeeRecord.EmployeeType == EmployeeType.Admin;
        }

        public bool IsUsernameExists(string bankId, string username)
        {
            return _bankContext.Employee.Any(e => e.BankId == bankId && e.Username == username && e.IsActive);
        }

        public void Authenticate(string employeeId, string password)
        {
            EmployeeDBModel employeeRecord = _bankContext.Employee.FirstOrDefault(e => e.Id == employeeId && e.IsActive);
            if (employeeRecord == null)
            {
                throw new EmployeeDoesNotExistException();
            }
            if (Convert.ToBase64String(employeeRecord.Password) != Convert.ToBase64String(_encryptionService.ComputeHash(password, employeeRecord.Salt)))
            {
                throw new AuthenticationFailedException();
            }
        }
    }
}
