using ATM.Models;
using ATM.Services.DBModels;
using ATM.Services.Exceptions;
using ATM.Services.IServices;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace ATM.Services
{
    public class EmployeeActionService : IEmployeeActionService
    {
        private readonly IMapper _mapper;
        private readonly BankContext _bankContext;

        public EmployeeActionService(BankContext bankContext, IMapper mapper)
        {
            _mapper = mapper;
            _bankContext = bankContext;
        }

        public void AddEmployeeAction(EmployeeAction employeeAction)
        {
            EmployeeActionDBModel employeeActionRecord = _mapper.Map<EmployeeActionDBModel>(employeeAction);
            _bankContext.EmployeeAction.Add(employeeActionRecord);
            _bankContext.SaveChanges();
        }

        public IList<EmployeeAction> GetEmployeeActions(string employeeId)
        {
            IList<EmployeeAction> actions;
            IList<EmployeeActionDBModel> actionRecords = _bankContext.EmployeeAction.Where(a => a.EmployeeId == employeeId).ToList();
            if (actionRecords.Count == 0 || actionRecords == null)
            {
                throw new NoEmployeeActionsException();
            }
            actions = _mapper.Map<EmployeeAction[]>(actionRecords);
            return actions;
        }
    }
}
