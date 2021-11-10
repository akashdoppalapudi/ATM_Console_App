using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM.Models;
using ATM.Models.Enums;
using ATM.Services.Exceptions;

namespace ATM.Services
{
    public class EmployeeActionService
    {
        private IList<EmployeeAction> employeeActions;
        private readonly DataService dataService;

        public EmployeeActionService()
        {
            dataService = new DataService();
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
    }
}
