using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace ATM
{
    public class Data
    {
        private const string FILEPATH = "../../../../accountsInfo.dat";
        public static BinaryFormatter formatter = new BinaryFormatter();

        public static List<Account> readAccounts()
        {
            if (File.Exists(FILEPATH))
            {
                FileStream readerFileStream = new FileStream(FILEPATH, FileMode.Open, FileAccess.Read);
                List<Account> accounts = (List<Account>)formatter.Deserialize(readerFileStream);
                readerFileStream.Close();
                return accounts;
            }
            else
            {
                List<Account> accounts = new List<Account>();
                return accounts;
            }
        }

        public static void writeAccounts(List<Account> accounts)
        {
            FileStream writerFileStream = new FileStream(FILEPATH, FileMode.Create, FileAccess.Write);
            formatter.Serialize(writerFileStream, accounts);
            writerFileStream.Close();
        }
    }
}
