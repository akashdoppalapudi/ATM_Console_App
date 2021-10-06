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
                    accountsHandler.createNewAccount(name, pin, accountType);
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
                        string operation = ConsoleUI.selectOperation();
                        if (operation == "1")
                        {
                            decimal amount = ConsoleUI.getAmount('d');
                            accountsHandler.deposit(selectedAcc, amount, userInputPin);
                            break;
                        }
                        else if (operation == "2")
                        {
                            decimal amount = ConsoleUI.getAmount('w');
                            accountsHandler.withdraw(selectedAcc, amount, userInputPin);
                            break;
                        }
                        else if (operation == "3")
                        {
                            decimal amount = ConsoleUI.getAmount('t');
                            Account transferToAccount = ConsoleUI.selectTransferToAccount(selectedAcc.accountNumber, allAccounts);
                            accountsHandler.transfer(selectedAcc, transferToAccount, amount, userInputPin);
                            break;
                        }
                        else if (operation == "4")
                        {
                            List<Transaction> transactions = accountsHandler.getTransactions(selectedAcc, userInputPin);
                            ConsoleUI.printTransactions(transactions, selectedAcc.availableBalance);
                            break;
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
