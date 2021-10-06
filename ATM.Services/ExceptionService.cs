using System;


namespace ATM.Services
{
    public class AccountCreationFailedException : Exception
    {
    }

    public class InvalidAmountException : Exception
    {
    }

    public class AuthenticationFailedException : Exception
    {
    }

    public class TransferFailedException : Exception
    {
    }
}
