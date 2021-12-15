using ATM.Models;
using ATM.Models.Enums;
using ATM.Services.DBModels;
using ATM.Services.Exceptions;
using ATM.Services.IServices;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATM.Services
{
    public class EmployeeActionService : IEmployeeActionService
    {
        private readonly IIDGenService _idGenService;
        private readonly MapperConfiguration employeeActionDBConfig;
        private readonly Mapper employeeActionDBMapper;
        private readonly MapperConfiguration dbEmployeeActionConfig;
        private readonly Mapper dbEmployeeActionMapper;
        private readonly BankContext _bankContext;

        public EmployeeActionService(IIDGenService idGenService, BankContext bankContext)
        {
            _idGenService = idGenService;
            employeeActionDBConfig = new MapperConfiguration(cfg => cfg.CreateMap<EmployeeAction, EmployeeActionDBModel>());
            employeeActionDBMapper = new Mapper(employeeActionDBConfig);
            dbEmployeeActionConfig = new MapperConfiguration(cfg => cfg.CreateMap<EmployeeActionDBModel, EmployeeAction>());
            dbEmployeeActionMapper = new Mapper(dbEmployeeActionConfig);
            _bankContext = bankContext;
        }

        public EmployeeAction CreateEmployeeAction(string bankId, string employeeId, EmployeeActionType actionType, string accId = null, string TXNID = null)
        {
            EmployeeAction newEmployeeAction = new EmployeeAction
            {
                Id = _idGenService.GenEmployeeActionId(bankId, employeeId),
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
            _bankContext.EmployeeAction.Add(employeeActionRecord);
            _bankContext.SaveChanges();
        }

        public IList<EmployeeAction> GetEmployeeActions(string bankId, string employeeId)
        {
            IList<EmployeeAction> actions;
            actions = dbEmployeeActionMapper.Map<EmployeeAction[]>(_bankContext.EmployeeAction.Where(a => a.BankId == bankId && a.EmployeeId == employeeId).ToList());
            if (actions.Count == 0 || actions == null)
            {
                throw new NoEmployeeActionsException();
            }
            return actions;
        }
    }
}
