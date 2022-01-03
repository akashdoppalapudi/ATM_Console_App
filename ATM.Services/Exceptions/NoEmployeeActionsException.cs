using System;

namespace ATM.Services.Exceptions
{
    public class NoEmployeeActionsException : Exception
    {
        public NoEmployeeActionsException() : base("There are no actions for this employee") { }
    }
}
