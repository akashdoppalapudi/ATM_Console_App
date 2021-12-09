namespace ATM.Services.IServices
{
    public interface IIDGenService
    {
        string GenEmployeeActionId(string bankId, string empId);
        string GenId(string Name);
        string GenTransactionId(string bankId, string accId);
    }
}