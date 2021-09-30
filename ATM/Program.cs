using ATM.Models;
using ATM.Services;
using System;

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
                    accountsHandler.createNewAccount();
                }
                else if (option == "2")
                {
                    while (true)
                    {
                        Account selectedAcc = ConsoleUI.selectAccount();
                        if (selectedAcc == null)
                        {
                            StandardMessages.invalidOptionMsg();
                            break;
                        }
                        string operation = ConsoleUI.selectOperation();
                        if (operation == "1")
                        {
                            accountsHandler.deposit(selectedAcc);
                            break;
                        }
                        else if (operation == "2")
                        {
                            accountsHandler.withdraw(selectedAcc);
                            break;
                        }
                        else if (operation == "3")
                        {
                            accountsHandler.transfer(selectedAcc);
                            break;
                        }
                        else if (operation == "4")
                        {
                            accountsHandler.transactionHistory(selectedAcc);
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
