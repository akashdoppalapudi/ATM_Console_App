using System;
using System.Collections.Generic;

namespace ATM
{
    [Serializable]
    class Account
    {
        public string name;
        public int accountNumber;
        private int availBal, pin;
        private List<string> transactions;

        public Account(int accNo)
        {
            Console.WriteLine("\n\n____CREATING ACCOUNT____");
            Console.Write("Enter Your name : ");
            this.name = Console.ReadLine();
            Console.Write("Set a 4-digit pin : ");
            string selectedPin = Console.ReadLine();
            if (selectedPin.Length != 4)
            {
                Console.WriteLine("Invalid Pin! Try again");
                this.pin = -1;
            }
            else
            {
                try
                {
                    this.pin = Convert.ToInt32(selectedPin);
                    this.accountNumber = accNo;
                    this.availBal = 0;
                    this.transactions = new List<string>();
                    string transaction = DateTime.Now.ToString() + "\tAccount Created";
                    transactions.Add(transaction);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid Pin! Try again");
                    this.pin = -1;
                }
            }
        }

        public bool isPinValid()
        {
            if(this.pin == -1)
            {
                return false;
            } else
            {
                return true;
            }
        }

        public void deposit()
        {
            Console.Write("\nEnter PIN : ");
            string enteredPin = Console.ReadLine();
            if (enteredPin == Convert.ToString(this.pin))
            {
                Console.WriteLine("\n\n____DEPOSIT____");
                Console.Write("Enter ammount to be deposited : ");
                int ammount = Convert.ToInt32(Console.ReadLine());
                if (ammount > 0)
                {
                    this.availBal += ammount;
                    string transaction = DateTime.Now.ToString() + "\tCredited    Rs. " + Convert.ToString(ammount);
                    this.transactions.Add(transaction);
                    Console.WriteLine("Ammount Deposited Successfully");
                }
                else
                {
                    Console.WriteLine("Invalid Ammount");
                }
            } else
            {
                Console.WriteLine("Wrong pin!");
            }
            
        }

        public void withdraw()
        {
            Console.Write("\nEnter PIN : ");
            string enteredPin = Console.ReadLine();
            if (enteredPin == Convert.ToString(this.pin))
            {
                Console.WriteLine("\n\n____WITHDRAW____");
                Console.Write("Enter ammount to be withdrawn : ");
                int ammount = Convert.ToInt32(Console.ReadLine());
                if (ammount > 0 && ammount <= this.availBal)
                {
                    this.availBal -= ammount;
                    string transaction = DateTime.Now.ToString() + "\tDebited     Rs. " + Convert.ToString(ammount);
                    this.transactions.Add(transaction);
                    Console.WriteLine("Ammount Withdrawn Successfully");
                }
                else
                {
                    Console.WriteLine("Invalid Amount");
                }
            } else
            {
                Console.WriteLine("Wrong Pin!");
            }
            
        }

        public int Tout()
        {
            Console.Write("\nEnter PIN : ");
            string enteredPin = Console.ReadLine();
            if (enteredPin == Convert.ToString(this.pin))
            {
                Console.WriteLine("\n\n____TRANSFER____");
                Console.Write("Enter ammount to be transfered : ");
                int ammount = Convert.ToInt32(Console.ReadLine());
                if (ammount > 0 && ammount <= this.availBal)
                {
                    this.availBal -= ammount;
                    string transaction = DateTime.Now.ToString() + "\tDebited     Rs. " + Convert.ToString(ammount);
                    this.transactions.Add(transaction);
                    return ammount;
                }
                else
                {
                    Console.WriteLine("Invalid Amount");
                    return -1;
                }
            }
            else
            {
                Console.WriteLine("Wrong Pin!");
                return -1;
            }
        }

        public void Tin(int amt)
        {
            this.availBal += amt;
            string transaction = DateTime.Now.ToString() + "\tCredited    Rs. " + Convert.ToString(amt);
            this.transactions.Add(transaction);
            Console.WriteLine("Ammount Transfered Successfully");
        }

        public void history()
        {
            Console.Write("\nEnter PIN : ");
            string enteredPin = Console.ReadLine();
            if (enteredPin == Convert.ToString(this.pin))
            {
                Console.WriteLine("\n\n____TRANSACTION HISTORY____");
                foreach (string transaction in transactions)
                {
                    Console.WriteLine(transaction);
                }
                Console.WriteLine("\t\tAvailable Balance : Rs. " + this.availBal);
            }
            else
            {
                Console.WriteLine("Wrong Pin!");
            }
        }
    }
}
