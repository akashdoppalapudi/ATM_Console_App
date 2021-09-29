using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
