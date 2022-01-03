using System;

namespace ATM.Services.Exceptions
{
    public class AccessDeniedException : Exception
    {
        public AccessDeniedException() : base("User don't have access to do this operation") { }
    }
}
