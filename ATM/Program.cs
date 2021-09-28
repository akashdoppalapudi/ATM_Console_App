using System;
using System.Collections.Generic;

namespace ATM
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                List<Account> accounts = Data.readAccounts();
                string option = ConsoleUI.welcome();
                if(option == "1")
                {
                    Account newAcc = Account.CreateInstance(accounts.Count+1);
                    if (newAcc!=null)
                    {
                        accounts.Add(newAcc);
                        Data.writeAccounts(accounts);
                    }
                    
                } else if (option == "2")
                {
                    while(true)
                    {
                        string selectedAcc = ConsoleUI.selectAccount(accounts);
                        int index;
                        try
                        {
                            index = Convert.ToInt32(selectedAcc) - 1;
                        } catch {
                            if (selectedAcc != "b")
                            {
                                ConsoleUI.invalidOptionMsg();
                                continue;
                            } else
                            {
                                break;
                            }
                        }
                        if (index >= 0 && index < accounts.Count)
                        {
                            string operation = ConsoleUI.selectOperation();
                            if (operation=="1")
                            {
                                accounts[index].deposit();
                                Data.writeAccounts(accounts);
                                break;
                            } else if (operation=="2")
                            {
                                accounts[index].withdraw();
                                Data.writeAccounts(accounts);
                                break;
                            } else if (operation=="3")
                            {
                                int amt = accounts[index].Tout();
                                Console.WriteLine();
                                foreach (Account acc in accounts)
                                {
                                    if (index != accounts.IndexOf(acc))
                                    {
                                        Console.WriteLine(acc.AccountNumber + ". " + acc.Name);
                                    }
                                }
                                Console.Write("Select an Account to transfer money to : ");
                                string choice = (Console.ReadLine());
                                try
                                {
                                    int transferTo = Convert.ToInt32(choice);
                                    if (amt > 0)
                                    {
                                        accounts[transferTo - 1].Tin(amt);
                                        Data.writeAccounts(accounts);
                                        break;
                                    } else
                                    {
                                        Console.WriteLine("Transfer Failed");
                                        accounts[index].Tin(amt);
                                        Console.WriteLine("Back to your own Account.");
                                        Data.writeAccounts(accounts);
                                        break;
                                    }
                                } catch {
                                    ConsoleUI.invalidOptionMsg();
                                    accounts[index].Tin(amt);
                                    Console.WriteLine("Back to your own Account.");
                                    Data.writeAccounts(accounts);
                                    continue;
                                }
 
                            } else if (operation=="4") {
                                accounts[index].history();
                                break;
                            } else if (operation=="b")
                            {
                                continue;
                            } else
                            {
                                ConsoleUI.invalidOptionMsg();
                                break;
                            }
                        } else
                        {
                            ConsoleUI.invalidOptionMsg();
                        }
                    }
                } else if (option == "e")
                {
                    break;
                } else
                {
                    ConsoleUI.invalidOptionMsg();
                }
            }
        }
    }
}
