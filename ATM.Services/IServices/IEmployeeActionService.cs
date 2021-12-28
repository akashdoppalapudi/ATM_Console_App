using ATM.Models;
using System.Collections.Generic;

namespace ATM.Services.IServices
{
    public interface IEmployeeActionService
    {
        void AddEmployeeAction(EmployeeAction employeeAction);
        IList<EmployeeAction> GetEmployeeActions(string employeeId);
    }
}