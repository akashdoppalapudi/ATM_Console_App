using ATM.Models;
using ATM.Services;
using System.Collections.Generic;

namespace ATM.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            StandardMessages.welcomeMsg();
            while (true)
            {
                AccountsHandler accountsHandler = new AccountsHandler();
                string option = ConsoleUI.existingOrCreate();
                if (option == "1")
                {
                    (string name, string pin, AccountType accountType) = ConsoleUI.getDataForAccountCreation();
                    try
                    {
                        accountsHandler.createNewAccount(name, pin, accountType);
                        StandardMessages.accountCreationSuccess();
                    }
                    catch (AccountCreationFailedException)
                    {
                        StandardMessages.accountCreationFailed();
                    }
                }
                else if (option == "2")
                {
                    while (true)
                    {
                        List<Account> allAccounts = accountsHandler.getAllAccounts();
                        Account selectedAcc = ConsoleUI.selectAccount(allAccounts);
                        if (selectedAcc == null)
                        {
                            StandardMessages.invalidOptionMsg();
                            break;
                        }
                        string userInputPin = ConsoleUI.getPinFromUser();
                        try
                        {
                            accountsHandler.authenticate(selectedAcc, userInputPin);
                        }
                        catch (AuthenticationFailedException)
                        {
                            StandardMessages.wrongPinMsg();
                            break;
                        }
                        string operation = ConsoleUI.selectOperation();
                        if (operation == "1")
                        {
                            decimal amount = ConsoleUI.getAmount('d');
                            try
                            {
                                accountsHandler.deposit(selectedAcc, amount);
                                StandardMessages.depositSuccess();
                            }
                            catch (InvalidAmountException)
                            {
                                StandardMessages.invalidAmountMsg();
                            }
                            break;
                        }
                        else if (operation == "2")
                        {
                            decimal amount = ConsoleUI.getAmount('w');
                            try
                            {
                                accountsHandler.withdraw(selectedAcc, amount);
                                StandardMessages.withdrawSuccess();
                            }
                            catch (InvalidAmountException)
                            {
                                StandardMessages.invalidAmountMsg();
                            }
                            break;
                        }
                        else if (operation == "3")
                        {
                            decimal amount = ConsoleUI.getAmount('t');
                            Account transferToAccount = ConsoleUI.selectTransferToAccount(selectedAcc.accountNumber, allAccounts);
                            try
                            {
                                accountsHandler.transfer(selectedAcc, transferToAccount, amount);
                                StandardMessages.transferSuccess();
                            }
                            catch (InvalidAmountException)
                            {
                                StandardMessages.invalidAmountMsg();
                            }
                            catch (TransferFailedException)
                            {
                                StandardMessages.transferFailed();
                            }
                            break;
                        }
                        else if (operation == "4")
                        {
                            List<Transaction> transactions = accountsHandler.getTransactions(selectedAcc);
                            ConsoleUI.printTransactions(transactions, selectedAcc.availableBalance);
                        }
                        else if (operation == "b")
                        {
                            continue;
                        }
                        else
                        {
                            StandardMessages.invalidOptionMsg();
                            break;
                        }
                    }
                }
                else if (option == "e")
                {
                    break;
                }
                else
                {
                    StandardMessages.invalidOptionMsg();
                }
            }
        }
    }
}
