using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ATM
{
    class Program
    {
        public const string FILEPATH = "../../../../accountsInfo.dat";
        public static BinaryFormatter formatter = new BinaryFormatter();
        static void Main(string[] args)
        {
            while(true)
            {
                List<Account> accounts = readAccounts();
                Console.WriteLine("\n\n____ATM____");
                Console.WriteLine("1. Create New Account\n2. Exixting User");
                Console.Write("Select an Option (Enter 'e' to exit) : ");
                string option = Console.ReadLine();
                if(option == "1")
                {
                    Account newAcc = new Account(accounts.Count+1);
                    if (newAcc.isPinValid())
                    {
                        accounts.Add(newAcc);
                        writeAccounts(accounts);
                    } else
                    {
                        newAcc = null;
                    }
                    
                } else if (option == "2")
                {
                    while(true)
                    {
                        Console.WriteLine("\n\n____EXISTING ACCOUNTS____");
                        foreach (Account acc in accounts)
                        {
                            Console.WriteLine(acc.accountNumber + ". " + acc.name);
                        }
                        Console.Write("Select your Account (Enter 'b' to go back) : ");
                        string selectedAcc = Console.ReadLine();
                        int index;
                        try
                        {
                            index = Convert.ToInt32(selectedAcc) - 1;
                        } catch (Exception e)
                        {
                            if (selectedAcc != "b")
                            {
                                Console.WriteLine("Invalid Option!");
                                continue;
                            } else
                            {
                                break;
                            }
                        }
                        if (index >= 0 && index < accounts.Count)
                        {
                            Console.WriteLine("\n\n____OPERATIONS____");
                            Console.WriteLine("\n1. Deposit\n2. Withdraw\n3. Transfer\n4. Show Transaction History");
                            Console.Write("Select an operation (Enter 'b' to go back) : ");
                            string operation = Console.ReadLine();
                            if (operation=="1")
                            {
                                accounts[index].deposit();
                                writeAccounts(accounts);
                                break;
                            } else if (operation=="2")
                            {
                                accounts[index].withdraw();
                                writeAccounts(accounts);
                                break;
                            } else if (operation=="3")
                            {
                                int amt = accounts[index].Tout();
                                Console.WriteLine();
                                foreach (Account acc in accounts)
                                {
                                    if (index != accounts.IndexOf(acc))
                                    {
                                        Console.WriteLine(acc.accountNumber + ". " + acc.name);
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
                                        writeAccounts(accounts);
                                        break;
                                    } else
                                    {
                                        Console.WriteLine("Transfer Failed");
                                        accounts[index].Tin(amt);
                                        Console.WriteLine("Back to your own Account.");
                                        writeAccounts(accounts);
                                        break;
                                    }
                                } catch (Exception e)
                                {
                                    Console.WriteLine("Invalid Choice!");
                                    accounts[index].Tin(amt);
                                    Console.WriteLine("Back to your own Account.");
                                    writeAccounts(accounts);
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
                                Console.WriteLine("invalid Choice");
                                break;
                            }
                        } else
                        {
                            Console.WriteLine("Invalid Option!");
                        }
                    }
                } else if (option == "e")
                {
                    Environment.Exit(0);
                } else
                {
                    Console.WriteLine("Invalid Option!");
                }
            }
        }

        public static List<Account> readAccounts()
        {
            if (File.Exists(FILEPATH))
            {
                FileStream readerFileStream = new FileStream(FILEPATH, FileMode.Open, FileAccess.Read);
                List<Account> accounts = (List<Account>)formatter.Deserialize(readerFileStream);
                readerFileStream.Close();
                return accounts;
            } else
            {
                List<Account> accounts = new List<Account>();
                return accounts;
            }
        }

        public static void writeAccounts(List<Account> accounts)
        {
            FileStream writerFileStream = new FileStream(FILEPATH, FileMode.Create, FileAccess.Write);
            formatter.Serialize(writerFileStream, accounts);
            writerFileStream.Close();
        }
    }
}
