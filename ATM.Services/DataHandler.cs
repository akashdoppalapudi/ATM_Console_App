using ATM.Models;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ATM.Services
{
    public class DataHandler
    {
        private const string FILEPATH = "../../../../bankdata.dat";
        private static BinaryFormatter formatter = new BinaryFormatter();

        public static Bank ReadBankData()
        {
            if (File.Exists(FILEPATH))
            {
                FileStream readerFileStream = new FileStream(FILEPATH, FileMode.Open, FileAccess.Read);
                Bank retrievedBank = (Bank)formatter.Deserialize(readerFileStream);
                readerFileStream.Close();
                return retrievedBank;
            }
            else
            {
                return null;
            }
        }

        public static void WriteBankData(Bank updatedBank)
        {
            FileStream writerFileStream = new FileStream(FILEPATH, FileMode.Create, FileAccess.Write);
            formatter.Serialize(writerFileStream, updatedBank);
            writerFileStream.Close();
        }
    }
}
