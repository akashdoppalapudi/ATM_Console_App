using ATM.Models;
using ATM.Models.Enums;
using System;
using System.Collections.Generic;

namespace ATM.Services
{
    public class EmployeeActionService
    {
        private readonly IDGenService idGenService;
        private readonly DBService dbService;

        public EmployeeActionService()
        {
            idGenService = new IDGenService();
            dbService = new DBService();
        }

        public EmployeeAction CreateEmployeeAction(string bankId, string employeeId, EmployeeActionType actionType, string accId = null, string TXNID = null)
        {
            EmployeeAction newEmployeeAction = new EmployeeAction
            {
                Id = idGenService.GenEmployeeActionId(bankId, employeeId),
                TXNId = TXNID,
                AccountId = accId,
                ActionDate = DateTime.Now,
                ActionType = actionType
            };
            return newEmployeeAction;
        }

        public void AddEmployeeAction(string bankId, string employeeId, EmployeeAction employeeAction)
        {
            employeeAction.BankId = bankId;
            employeeAction.EmployeeId = employeeId;
            dbService.AddEmployeeAction(employeeAction);
        }

        public IList<EmployeeAction> GetEmployeeActions(string bankId, string employeeId)
        {
            return dbService.GetEmployeeActions(bankId, employeeId);
        }
    }
}
