using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM.Models;

namespace ATM.Services
{
    public class TransactionHandler
    {
        public static Transaction accountCreationTransaction()
        {
            Transaction newTransaction = new Transaction
            {
                timeStamp = DateTime.Now,
                transactionAmount = 1500,
                transactionType = (TransactionType)1
            };

            return newTransaction;
        }
    }
}
