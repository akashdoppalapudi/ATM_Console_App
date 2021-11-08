using ATM.Models;
using ATM.Models.Enums;
using System;

namespace ATM.Services
{
    public class EmployeeService
    {
        private IDGenService idGenService;
        private EncryptionService encryptionService;
        public EmployeeService()
        {
            idGenService = new IDGenService();
            encryptionService = new EncryptionService();
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

        public void AddAction(Employee employee, EmployeeAction action)
        {
            employee.EmployeeActions.Add(action);
            employee.UpdatedOn = DateTime.Now;
        }

        public void UpdateEmployee(Employee employee, Employee UpdateEmployee)
        {
            employee.Name = UpdateEmployee.Name;
            employee.Gender = UpdateEmployee.Gender;
            employee.Username = UpdateEmployee.Username;
            if (UpdateEmployee.Password != encryptionService.ComputeSha256Hash(""))
            {
                employee.Password = UpdateEmployee.Password;
            }
            employee.EmployeeType = UpdateEmployee.EmployeeType;
            employee.UpdatedOn = DateTime.Now;
        }

        public void DeleteEmployee(Employee employee)
        {
            employee.IsActive = false;
            employee.UpdatedOn = DateTime.Now;
            employee.DeletedOn = DateTime.Now;
        }

        public bool IsEmployeeAdmin(Employee employee)
        {
            return employee.EmployeeType == EmployeeType.Admin;
        }

        public bool Authenticate(Employee employee, string password)
        {
            return employee.Password == encryptionService.ComputeSha256Hash(password);
        }
    }
}
