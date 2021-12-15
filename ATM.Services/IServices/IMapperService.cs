using ATM.Models;
using ATM.Services.DBModels;

namespace ATM.Services.IServices
{
    public interface IMapperService
    {
        AccountDBModel MapAccountToDB(Account account);
        BankDBModel MapBankToDB(Bank bank);
        CurrencyDBModel MapCurrencyToDB(Currency currency);
        EmployeeAction MapDBToEmployeeAction(EmployeeActionDBModel employeeActionDBModel);
        Account MapDBToAccount(AccountDBModel accountDBModel);
        Bank MapDBToBank(BankDBModel bankModel);
        Currency MapDBToCurrency(CurrencyDBModel currencyDBModel);
        Employee MapDBToEmployee(EmployeeDBModel employeeDbModel);
        Transaction MapDBToTransaction(TransactionDBModel transactionDBModel);
        EmployeeActionDBModel MapEmployeeActionToDB(EmployeeAction employeeAction);
        EmployeeDBModel MapEmployeeToDB(Employee employee);
        TransactionDBModel MapTransctionToDB(Transaction transaction);
    }
}