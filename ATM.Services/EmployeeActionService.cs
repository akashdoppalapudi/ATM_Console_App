using ATM.Models;
using ATM.Models.Enums;
using ATM.Services.DBModels;
using ATM.Services.Exceptions;
using ATM.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATM.Services
{
    public class EmployeeActionService : IEmployeeActionService
    {
        private readonly IMapperService _mapperService;
        private readonly BankContext _bankContext;

        public EmployeeActionService(BankContext bankContext, IMapperService mapperService)
        {
            _mapperService = mapperService;
            _bankContext = bankContext;
        }

        public EmployeeAction CreateEmployeeAction(string bankId, string employeeId, EmployeeActionType actionType, string accId = null, string TXNID = null)
        {
            EmployeeAction newEmployeeAction = new EmployeeAction
            {
                Id = bankId.GenEmployeeActionId(employeeId),
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
            EmployeeActionDBModel employeeActionRecord = _mapperService.MapEmployeeActionToDB(employeeAction);
            _bankContext.EmployeeAction.Add(employeeActionRecord);
            _bankContext.SaveChanges();
        }

        public IList<EmployeeAction> GetEmployeeActions(string bankId, string employeeId)
        {
            IList<EmployeeAction> actions = new List<EmployeeAction>();
            IList<EmployeeActionDBModel> actionRecords = _bankContext.EmployeeAction.Where(a => a.BankId == bankId && a.EmployeeId == employeeId).ToList();
            foreach (EmployeeActionDBModel adb in actionRecords)
            {
                actions.Add(_mapperService.MapDBToEmployeeAction(adb));
            }
            if (actions.Count == 0 || actions == null)
            {
                throw new NoEmployeeActionsException();
            }
            return actions;
        }
    }
}
