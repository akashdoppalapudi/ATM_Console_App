using System;
using System.Collections.Generic;

namespace ATM
{
    [Serializable]
    public class Account
    {
        private string name;
        private int availBal, pin, accountNumber;
        private List<string> transactions;

        private Account(int accNo, int selectedPin, string selectedName)
        {
            this.accountNumber = accNo;
            this.name = selectedName;
            this.pin = selectedPin;
            this.availBal = 0;
            transactions = new List<string>();
            string transaction = DateTime.Now.ToString() + "\tAccount Created";
            transactions.Add(transaction);
        }

        public static Account CreateInstance(int accNo)
        {
            (string selectedName, int selectedPin) = ConsoleUI.accountCreationInfo();
            if (selectedName == "____" || selectedPin == -1)
            {
                return null;
            }
            return new Account(accNo, selectedPin, selectedName);
        }

        public string Name
        {
            get => this.name;
        }

        public int AccountNumber
        {
            get => this.accountNumber;
        }

        public void deposit()
        {
            if (authenticate())
            {
                int amount = ConsoleUI.getAmmount('d');
                if (amount > 0)
                {
                    this.availBal += amount;
                    string transaction = DateTime.Now.ToString() + "\tCredited    Rs. " + Convert.ToString(amount);
                    this.transactions.Add(transaction);
                    ConsoleUI.succesMsg('d');
                }
                else
                {
                    ConsoleUI.invalidAmountMsg();
                }
            }
        }

        public void withdraw()
        {
            if (authenticate())
            {
                int amount = ConsoleUI.getAmmount('w');
                if (amount > 0 && amount <= this.availBal)
                {
                    this.availBal -= amount;
                    string transaction = DateTime.Now.ToString() + "\tDebited     Rs. " + Convert.ToString(amount);
                    this.transactions.Add(transaction);
                    ConsoleUI.succesMsg('w');
                }
                else
                {
                    ConsoleUI.invalidAmountMsg();
                }
            }
        }

        public void Tout(List<Account> accounts)
        {
            if (authenticate())
            {
                int amount = ConsoleUI.getAmmount('t');
                Account Toaccount = ConsoleUI.selectTransferToAccount(accounts, this.AccountNumber);
                if (amount > 0 && amount <= this.availBal)
                {
                    if (Toaccount != null)
                    {
                        this.availBal -= amount;
                        string transaction = DateTime.Now.ToString() + "\tDebited     Rs. " + Convert.ToString(amount);
                        this.transactions.Add(transaction);
                        Toaccount.Tin(amount);
                    }
                    else
                    {
                        ConsoleUI.transferFailedMsg();
                    }
                }
                else
                {
                    ConsoleUI.invalidAmountMsg();
                }
            }
        }

        public void Tin(int amt)
        {
            this.availBal += amt;
            string transaction = DateTime.Now.ToString() + "\tCredited    Rs. " + Convert.ToString(amt);
            this.transactions.Add(transaction);
            ConsoleUI.succesMsg('t');
        }

        public void history()
        {
            if (authenticate())
            {
                Console.WriteLine("\n\n____TRANSACTION HISTORY____");
                foreach (string transaction in this.transactions)
                {
                    Console.WriteLine(transaction);
                }
                Console.WriteLine("\t\tAvailable Balance : Rs. " + this.availBal);
            }

        }

        private bool authenticate()
        {
            string enteredPin = ConsoleUI.getPinFromUser();
            if (enteredPin == Convert.ToString(this.pin))
            {
                return true;
            }
            ConsoleUI.wrongPinMsg();
            return false;
        }
    }
}
