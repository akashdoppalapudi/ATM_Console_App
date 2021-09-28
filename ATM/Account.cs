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
            if (authenticate())
            {
                Console.WriteLine("\n\n____DEPOSIT____");
                Console.Write("Enter Amount to be deposited : ");
                int Amount = Convert.ToInt32(Console.ReadLine());
                if (Amount > 0)
                {
                    this.availBal += Amount;
                    string transaction = DateTime.Now.ToString() + "\tCredited    Rs. " + Convert.ToString(Amount);
                    this.transactions.Add(transaction);
                    Console.WriteLine("Amount Deposited Successfully");
                }
                else
                {
                    Console.WriteLine("Invalid Amount");
                }
            }
        }

        public void withdraw()
        {
            if (authenticate())
            {
                Console.WriteLine("\n\n____WITHDRAW____");
                Console.Write("Enter Amount to be withdrawn : ");
                int Amount = Convert.ToInt32(Console.ReadLine());
                if (Amount > 0 && Amount <= this.availBal)
                {
                    this.availBal -= Amount;
                    string transaction = DateTime.Now.ToString() + "\tDebited     Rs. " + Convert.ToString(Amount);
                    this.transactions.Add(transaction);
                    Console.WriteLine("Amount Withdrawn Successfully");
                }
                else
                {
                    Console.WriteLine("Invalid Amount");
                }
            } 
        }

        public int Tout()
        {
            if (authenticate())
            {
                Console.WriteLine("\n\n____TRANSFER____");
                Console.Write("Enter Amount to be transfered : ");
                int Amount = Convert.ToInt32(Console.ReadLine());
                if (Amount > 0 && Amount <= this.availBal)
                {
                    this.availBal -= Amount;
                    string transaction = DateTime.Now.ToString() + "\tDebited     Rs. " + Convert.ToString(Amount);
                    this.transactions.Add(transaction);
                    return Amount;
                }
                else
                {
                    Console.WriteLine("Invalid Amount");
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }

        public void Tin(int amt)
        {
            this.availBal += amt;
            string transaction = DateTime.Now.ToString() + "\tCredited    Rs. " + Convert.ToString(amt);
            this.transactions.Add(transaction);
            Console.WriteLine("Amount Transfered Successfully");
        }

        public void history()
        {
            if (authenticate())
            {
                Console.WriteLine("\n\n____TRANSACTION HISTORY____");
                foreach (string transaction in transactions)
                {
                    Console.WriteLine(transaction);
                }
                Console.WriteLine("\t\tAvailable Balance : Rs. " + this.availBal);
            }
            
        }

        private bool authenticate()
        {
            Console.WriteLine("\n____AUTHENTICATION____");
            Console.Write("\nEnter PIN : ");
            string enteredPin = Console.ReadLine();
            if (enteredPin==Convert.ToString(this.pin))
            {
                return true;
            }
            Console.WriteLine("Wrong Pin!");
            return false;
        }
    }
}
