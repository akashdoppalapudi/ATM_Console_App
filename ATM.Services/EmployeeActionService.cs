using ATM.Models;
using ATM.Models.Enums;
using System;
using ATM.Services.DBModels;
using ATM.Services.Exceptions;
using System.Collections.Generic;
using AutoMapper;
using System.Linq;

namespace ATM.Services
{
    public class EmployeeActionService
    {
        private readonly IDGenService idGenService;
        private readonly MapperConfiguration employeeActionDBConfig;
        private readonly Mapper employeeActionDBMapper;

        public EmployeeActionService()
        {
            idGenService = new IDGenService();
            employeeActionDBConfig = new MapperConfiguration(cfg => cfg.CreateMap<EmployeeAction, EmployeeActionDBModel>());
            employeeActionDBMapper = new Mapper(employeeActionDBConfig);
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
            EmployeeActionDBModel employeeActionRecord = employeeActionDBMapper.Map<EmployeeActionDBModel>(employeeAction);
            using (BankContext bankContext = new BankContext())
            {
                bankContext.EmployeeAction.Add(employeeActionRecord);
                bankContext.SaveChanges();
            }
        }

        public IList<EmployeeAction> GetEmployeeActions(string bankId, string employeeId)
        {
            IList<EmployeeAction> actions;
            using (BankContext bankContext = new BankContext())
            {
                actions = employeeActionDBMapper.Map<EmployeeAction[]>(bankContext.EmployeeAction.Where(a => a.BankId == bankId && a.EmployeeId == employeeId).ToList());
            }
            if (actions.Count == 0 || actions == null)
            {
                throw new NoEmployeeActionsException();
            }
            return actions;
        }
    }
}
