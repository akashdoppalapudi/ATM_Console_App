using ATM.Models;
using ATM.Models.Enums;
using ATM.Services;
using ATM.Services.Exceptions;
using System;
using System.Collections.Generic;

namespace ATM.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleUI consoleUI = new ConsoleUI();
            ConsoleMessages consoleMessages = new ConsoleMessages();
            BankService bankService;
            consoleMessages.WelcomeMsg();
            while (true)
            {
                bankService = new BankService();
                (Tuple<string> bankDetails, Tuple<string, Gender, string, string> employeeDetails)  = consoleUI.GetDataForBankCreation();
                bankService.CreateNewBank(bankDetails, employeeDetails);
                Dictionary<string, string> bankNames = bankService.GetAllBankNames();
                consoleUI.SelectBank(bankNames);
                break;
            }
        }
    }
}
