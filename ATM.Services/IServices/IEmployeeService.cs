using ATM.Models;
using ATM.Models.Enums;

namespace ATM.Services.IServices
{
    public interface IEmployeeService
    {
        void AddEmployee(string bankId, Employee employee);
        void Authenticate(string bankId, string employeeId, string password);
        void CheckEmployeeExistance(string bankId, string employeeId);
        Employee CreateEmployee(string name, Gender gender, string username, string password, EmployeeType employeeType);
        void DeleteEmployee(string bankId, string employeeId);
        Employee GetEmployeeDetails(string bankId, string employeeId);
        string GetEmployeeIdByUsername(string bankId, string username);
        bool IsEmployeeAdmin(string bankId, string employeeId);
        void UpdateEmployee(string bankId, string employeeId, Employee UpdateEmployee);
        void ValidateUsername(string bankId, string username);
    }
}