using System;
using System.Collections.Generic;
using ATM.Services;
using ATM.Models;

namespace ATM.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            AccountsHandler accountsHandler = new AccountsHandler();
            accountsHandler.createNewAccount();
            Account account = ConsoleUI.selectAccount();
            if (account==null)
            {
                Console.WriteLine("Invalid Coice");
            }
            else
            {
                accountsHandler.deposit(account);
                accountsHandler.withdraw(account);
            }
            //while (true)
            //{
            //    List<Account> accounts = Data.readAccounts();
            //    string option = ConsoleUI.welcome();
            //    if (option == "1")
            //    {
            //        Account newAcc = Account.CreateInstance(accounts.Count + 1);
            //        if (newAcc != null)
            //        {
            //            accounts.Add(newAcc);
            //            Data.writeAccounts(accounts);
            //        }
            //    }
            //    else if (option == "2")
            //    {
            //        while (true)
            //        {
            //            string selectedAcc = ConsoleUI.selectAccount(accounts);
            //            int index;
            //            try
            //            {
            //                index = Convert.ToInt32(selectedAcc) - 1;
            //            }
            //            catch
            //            {
            //                if (selectedAcc != "b")
            //                {
            //                    ConsoleUI.invalidOptionMsg();
            //                    continue;
            //                }
            //                else
            //                {
            //                    break;
            //                }
            //            }
            //            if (index >= 0 && index < accounts.Count)
            //            {
            //                string operation = ConsoleUI.selectOperation();
            //                if (operation == "1")
            //                {
            //                    accounts[index].deposit();
            //                    Data.writeAccounts(accounts);
            //                    break;
            //                }
            //                else if (operation == "2")
            //                {
            //                    accounts[index].withdraw();
            //                    Data.writeAccounts(accounts);
            //                    break;
            //                }
            //                else if (operation == "3")
            //                {
            //                    accounts[index].Tout(accounts);
            //                    Data.writeAccounts(accounts);
            //                    break;
            //                }
            //                else if (operation == "4")
            //                {
            //                    accounts[index].history();
            //                    break;
            //                }
            //                else if (operation == "b")
            //                {
            //                    continue;
            //                }
            //                else
            //                {
            //                    ConsoleUI.invalidOptionMsg();
            //                    break;
            //                }
            //            }
            //            else
            //            {
            //                ConsoleUI.invalidOptionMsg();
            //            }
            //        }
            //    }
            //    else if (option == "e")
            //    {
            //        break;
            //    }
            //    else
            //    {
            //        ConsoleUI.invalidOptionMsg();
            //    }
            //}
        }
    }
}
