using ATM.Models;
using ATM.Models.Enums;
using ATM.Services.Exceptions;
using AutoMapper;
using ATM.Services.DBModels;
using System;
using System.Linq;

namespace ATM.Services
{
    public class EmployeeService
    {
        private readonly IDGenService idGenService;
        private readonly EncryptionService encryptionService;
        private readonly MapperConfiguration employeeDBConfig;
        private readonly MapperConfiguration dbEmployeeConfig;
        private readonly Mapper employeeDBMapper;
        private readonly Mapper dbEmployeeMapper;

        public EmployeeService()
        {
            idGenService = new IDGenService();
            encryptionService = new EncryptionService();
            employeeDBConfig = new MapperConfiguration(cfg => cfg.CreateMap<Employee, EmployeeDBModel>());
            employeeDBMapper = new Mapper(employeeDBConfig);
            dbEmployeeConfig = new MapperConfiguration(cfg => cfg.CreateMap<EmployeeDBModel, Employee>());
            dbEmployeeMapper = new Mapper(dbEmployeeConfig);
        }

        private Employee GetEmployeeById(string bankId, string employeeId)
        {
            CheckEmployeeExistance(bankId, employeeId);
            using (BankContext bankContext = new BankContext())
            {
                EmployeeDBModel employeeRecord = bankContext.Employee.FirstOrDefault(e => e.BankId == bankId && e.Id == employeeId && e.IsActive);
                return dbEmployeeMapper.Map<Employee>(employeeRecord);
            }
        }

        public void CheckEmployeeExistance(string bankId, string employeeId)
        {
            using (BankContext bankContext = new BankContext())
            {
                if (!bankContext.Employee.Any(e => e.BankId == bankId && e.Id == employeeId && e.IsActive))
                {
                    throw new EmployeeDoesNotExistException();
                }
            }
        }

        public Employee CreateEmployee(string name, Gender gender, string username, string password, EmployeeType employeeType)
        {
            (byte[] passwordBytes, byte[] saltBytes) = encryptionService.ComputeHash(password);
            return new Employee
            {
                Id = idGenService.GenId(name),
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
            using (BankContext bankContext = new BankContext())
            {
                EmployeeDBModel employeeRecord = bankContext.Employee.FirstOrDefault(e => e.BankId == bankId && e.IsActive && e.Username == username);
                if (employeeRecord == null)
                {
                    throw new EmployeeDoesNotExistException();
                }
                id = employeeRecord.Id;
            }
            return id;
        }

        public void AddEmployee(string bankId, Employee employee)
        {
            employee.BankId = bankId;
            EmployeeDBModel employeeRecord = employeeDBMapper.Map<EmployeeDBModel>(employee);
            using (BankContext bankContext = new BankContext())
            {
                bankContext.Employee.Add(employeeRecord);
                bankContext.SaveChanges();
            }

        }

        public void UpdateEmployee(string bankId, string employeeId, Employee UpdateEmployee)
        {
            Employee employee = GetEmployeeById(bankId, employeeId);
            employee.Name = UpdateEmployee.Name;
            employee.Gender = UpdateEmployee.Gender;
            employee.Username = UpdateEmployee.Username;
            if (UpdateEmployee.Password != encryptionService.ComputeHash("", UpdateEmployee.Salt))
            {
                employee.Password = UpdateEmployee.Password;
                employee.Salt = UpdateEmployee.Salt;
            }
            employee.EmployeeType = UpdateEmployee.EmployeeType;
            using (BankContext bankContext = new BankContext())
            {
                EmployeeDBModel currentEmployeeRecord = bankContext.Employee.First(e => e.BankId == employee.BankId && e.Id == employee.Id && e.IsActive);
                currentEmployeeRecord.Name = employee.Name;
                currentEmployeeRecord.Gender = employee.Gender;
                currentEmployeeRecord.Username = employee.Username;
                currentEmployeeRecord.Password = employee.Password;
                currentEmployeeRecord.Salt = employee.Salt;
                currentEmployeeRecord.EmployeeType = employee.EmployeeType;
                bankContext.SaveChanges();
            }
        }

        public void DeleteEmployee(string bankId, string employeeId)
        {
            CheckEmployeeExistance(bankId, employeeId);
            using (BankContext bankContext = new BankContext())
            {
                EmployeeDBModel employeeRecord = bankContext.Employee.First(e => e.Id == employeeId && e.BankId == bankId && e.IsActive);
                employeeRecord.IsActive = false;
                employeeRecord.DeletedOn = DateTime.Now;
                bankContext.SaveChanges();
            }
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
            using (BankContext bankContext = new BankContext())
            {
                if (bankContext.Employee.Any(e => e.BankId == bankId && e.Username == username && e.IsActive))
                {
                    throw new UsernameAlreadyExistsException();
                }
            }
        }

        public void Authenticate(string bankId, string employeeId, string password)
        {
            Employee employee = GetEmployeeById(bankId, employeeId);
            if (Convert.ToBase64String(employee.Password) != Convert.ToBase64String(encryptionService.ComputeHash(password, employee.Salt)))
            {
                throw new AuthenticationFailedException();
            }
        }
    }
}
