using ATM.Models;
using ATM.Models.Enums;
using ATM.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATM.Services
{
    public class EmployeeService
    {
        private IList<Employee> employees;
        private readonly IDGenService idGenService;
        private readonly EncryptionService encryptionService;
        private readonly DataService dataService;

        public EmployeeService()
        {
            idGenService = new IDGenService();
            encryptionService = new EncryptionService();
            dataService = new DataService();
            PopulateEmployeeData();
        }

        private void PopulateEmployeeData()
        {
            this.employees = dataService.ReadEmployeeData();
            if (this.employees == null)
            {
                this.employees = new List<Employee>();
            }
        }

        private Employee GetEmployeeById(string bankId, string employeeId)
        {
            CheckEmployeeExistance(bankId, employeeId);
            return this.employees.FirstOrDefault(e => e.Id == employeeId && e.BankId == bankId && e.IsActive);
        }

        public Employee CreateEmployee(string name, Gender gender, string username, string password, EmployeeType employeeType)
        {
            return new Employee
            {
                Id = idGenService.GenId(name),
                Name = name,
                Gender = gender,
                Username = username,
                Password = encryptionService.ComputeSha256Hash(password),
                EmployeeType = employeeType
            };
        }

        public void CheckEmployeeExistance(string bankId, string employeeId)
        {
            try
            {
                BankService.CheckBankExistance(bankId);
                PopulateEmployeeData();
                if (this.employees.Any(e => e.Id == employeeId && e.BankId == bankId && e.IsActive))
                {
                    return;
                }
                throw new EmployeeDoesNotExistException();
            }
            catch (BankDoesnotExistException)
            {
                throw new EmployeeDoesNotExistException();
            }
        }

        public string GetEmployeeIdByUsername(string bankId, string username)
        {
            PopulateEmployeeData();
            Employee employee = this.employees.FirstOrDefault(e => e.IsActive && e.BankId == bankId && e.Username == username);
            if (employee == null)
            {
                throw new EmployeeDoesNotExistException();
            }
            return employee.Id;
        }

        public void AddEmployee(string bankId, Employee employee)
        {
            PopulateEmployeeData();
            employee.BankId = bankId;
            this.employees.Add(employee);
            dataService.WriteEmployeeData(this.employees);
        }

        public void UpdateEmployee(string bankId, string employeeId, Employee UpdateEmployee)
        {
            PopulateEmployeeData();
            Employee employee = GetEmployeeById(bankId, employeeId);
            employee.Name = UpdateEmployee.Name;
            employee.Gender = UpdateEmployee.Gender;
            employee.Username = UpdateEmployee.Username;
            if (UpdateEmployee.Password != encryptionService.ComputeSha256Hash(""))
            {
                employee.Password = UpdateEmployee.Password;
            }
            employee.EmployeeType = UpdateEmployee.EmployeeType;
            dataService.WriteEmployeeData(this.employees);
        }

        public void DeleteEmployee(string bankId, string employeeId)
        {
            PopulateEmployeeData();
            Employee employee = GetEmployeeById(bankId, employeeId);
            employee.IsActive = false;
            employee.DeletedOn = DateTime.Now;
            dataService.WriteEmployeeData(this.employees);
        }

        public Employee GetEmployeeDetails(string bankId, string employeeId)
        {
            PopulateEmployeeData();
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
            PopulateEmployeeData();
            if (this.employees.Any(e => e.BankId == bankId && e.Username == username && e.IsActive))
            {
                throw new UsernameAlreadyExistsException();
            }
        }

        public void Authenticate(string bankId, string employeeId, string password)
        {
            PopulateEmployeeData();
            Employee employee = GetEmployeeById(bankId, employeeId);
            if (employee.Password != encryptionService.ComputeSha256Hash(password))
            {
                throw new AuthenticationFailedException();
            }
        }
    }
}
