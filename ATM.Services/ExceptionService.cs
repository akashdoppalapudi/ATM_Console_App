using System;


namespace ATM.Services
{
    public class AccountCreationFailedException : Exception { }

    public class InvalidAmountException : Exception { }

    public class AuthenticationFailedException : Exception { }

    public class UserNotFoundException : Exception { }

    public class TransferFailedException : Exception { }

    public class BankCreationFailedException : Exception { }

    public class BankNameAlreadyExistsException : Exception { }

    public class BankDoesnotExistException : Exception { }

    public class UsernameAlreadyExistsException : Exception { }
}
