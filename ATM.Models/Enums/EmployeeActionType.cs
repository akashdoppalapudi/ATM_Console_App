using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Models.Enums
{
    public enum EmployeeActionType
    {
        NewAccount = 1,
        UpdateAccount,
        DeleteAccount,
        ServiceChargeUpdate,
        RevertTransaction
    }
}
