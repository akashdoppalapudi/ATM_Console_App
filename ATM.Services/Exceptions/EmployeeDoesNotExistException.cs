using System;

namespace ATM.Services.Exceptions
{
    public class EmployeeDoesNotExistException : Exception
    {
        public EmployeeDoesNotExistException() : base("Employee not found") { }
    }
}
