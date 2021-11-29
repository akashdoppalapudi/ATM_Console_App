using System.Security.Cryptography;
using System.Text;

namespace ATM.Services
{
    public class EncryptionService
    {
        public const int SALT_SIZE = 24;
        public const int HASH_SIZE = 64;
        public const int ITERATIONS = 10000;

        public byte[] ComputeHash(string rawData, byte[] salt)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(rawData, salt, ITERATIONS);
            byte[] hash = pbkdf2.GetBytes(HASH_SIZE);
            return hash;
        }

        public (byte[], byte[]) ComputeHash(string rawData)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SALT_SIZE];
            provider.GetBytes(salt);
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(rawData, salt, ITERATIONS);
            byte[] hash = pbkdf2.GetBytes(HASH_SIZE);
            return (hash, salt);
        }
    }
}
