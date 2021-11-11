using ATM.Models;
using ATM.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATM.Services
{
    public class EmployeeActionService
    {
        private IList<EmployeeAction> employeeActions;
        private readonly DataService dataService;
        private readonly IDGenService idGenService;

        public EmployeeActionService()
        {
            dataService = new DataService();
            idGenService = new IDGenService();
            PopulateEmployeeActionData();
        }

        private void PopulateEmployeeActionData()
        {
            this.employeeActions = dataService.ReadEmployeeActionData();
            if (this.employeeActions == null)
            {
                this.employeeActions = new List<EmployeeAction>();
            }
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
            PopulateEmployeeActionData();
            employeeAction.BankId = bankId;
            employeeAction.EmployeeId = employeeId;
            this.employeeActions.Add(employeeAction);
            dataService.WriteEmployeeActionData(this.employeeActions);
        }

        public IList<EmployeeAction> GetEmployeeActions(string bankId, string employeeId)
        {
            PopulateEmployeeActionData();
            return (IList<EmployeeAction>)this.employeeActions.Where(a => a.BankId == bankId && a.EmployeeId == employeeId);
        }
    }
}
