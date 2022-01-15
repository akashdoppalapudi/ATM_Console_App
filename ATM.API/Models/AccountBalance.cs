namespace ATM.API.Models
{
    public class AccountBalance
    {
        public decimal Balance { get; set; }

        public AccountBalance(decimal balance)
        {
            Balance = balance;
        }
    }
}
