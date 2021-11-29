using ATM.Models;
using ATM.Models.Enums;
using ATM.Services.Exceptions;

namespace ATM.Services
{
    public class EmployeeService
    {
        private readonly IDGenService idGenService;
        private readonly EncryptionService encryptionService;
        private readonly DBService dbService;

        public EmployeeService()
        {
            idGenService = new IDGenService();
            encryptionService = new EncryptionService();
            dbService = new DBService();
        }

        private Employee GetEmployeeById(string bankId, string employeeId)
        {
            return dbService.GetEmployeeById(bankId, employeeId);
        }

        public void CheckEmployeeExistance(string bankId, string employeeId)
        {
            dbService.CheckEmployeeExistance(bankId, employeeId);
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
            return dbService.GetEmployeeIdByUsername(bankId, username);
        }

        public void AddEmployee(string bankId, Employee employee)
        {
            employee.BankId = bankId;
            dbService.AddEmployee(employee);

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
            dbService.UpdateEmployee(employee);
        }

        public void DeleteEmployee(string bankId, string employeeId)
        {
            CheckEmployeeExistance(bankId, employeeId);
            dbService.DeletePerson(employeeId);
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
            dbService.ValidateEmployeeUsername(bankId, username);
        }

        public void Authenticate(string bankId, string employeeId, string password)
        {
            Employee employee = GetEmployeeById(bankId, employeeId);
            if (employee.Password != encryptionService.ComputeHash(password, employee.Salt))
            {
                throw new AuthenticationFailedException();
            }
        }
    }
}
