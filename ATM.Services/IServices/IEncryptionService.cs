namespace ATM.Services.IServices
{
    public interface IEncryptionService
    {
        (byte[], byte[]) ComputeHash(string rawData);
        byte[] ComputeHash(string rawData, byte[] salt);
    }
}