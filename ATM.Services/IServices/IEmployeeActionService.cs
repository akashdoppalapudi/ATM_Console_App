using ATM.Models;
using ATM.Models.Enums;
using System.Collections.Generic;

namespace ATM.Services.IServices
{
    public interface IEmployeeActionService
    {
        void AddEmployeeAction(string bankId, string employeeId, EmployeeAction employeeAction);
        EmployeeAction CreateEmployeeAction(string bankId, string employeeId, EmployeeActionType actionType, string accId = null, string TXNID = null);
        IList<EmployeeAction> GetEmployeeActions(string bankId, string employeeId);
    }
}