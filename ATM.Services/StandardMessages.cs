using System;

namespace ATM.Services
{
    public class StandardMessages
    {
        public static void accountCreationFailed()
        {
            Console.WriteLine("Account Creation Failed");
        }

        public static void accountCreationSuccess()
        {
            Console.WriteLine("Account Created Successfully");
        }

        public static void availableBalanceMsg(decimal availBal)
        {
            Console.WriteLine("\t\tAvailable Balance : " + availBal);
        }
    }
}
