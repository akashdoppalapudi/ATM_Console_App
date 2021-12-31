using ATM.Models;
using ATM.Models.ViewModels;

namespace ATM.Services.IServices
{
    public interface IEmployeeService
    {
        void AddEmployee(Employee employee);
        void Authenticate(string employeeId, string password);
        void DeleteEmployee(string employeeId);
        EmployeeViewModel GetEmployeeDetails(string employeeId);
        string GetEmployeeIdByUsername(string bankId, string username);
        bool IsEmployeeAdmin(string employeeId);
        void UpdateEmployee(string employeeId, Employee UpdateEmployee);
        bool IsUsernameExists(string bankId, string username);
    }
}