using ATM.Models;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ATM.Services
{
    public class DataHandler
    {
        private const string FILEPATH = "../../../../bankdata.dat";
        private BinaryFormatter formatter = new BinaryFormatter();

        public List<Bank> ReadBankData()
        {
            if (File.Exists(FILEPATH))
            {
                FileStream readerFileStream = new FileStream(FILEPATH, FileMode.Open, FileAccess.Read);
                List<Bank> retrievedBanks = (List<Bank>)formatter.Deserialize(readerFileStream);
                readerFileStream.Close();
                return retrievedBanks;
            }
            else
            {
                return null;
            }
        }

        public void WriteBankData(List<Bank> updatedBanks)
        {
            FileStream writerFileStream = new FileStream(FILEPATH, FileMode.Create, FileAccess.Write);
            formatter.Serialize(writerFileStream, updatedBanks);
            writerFileStream.Close();
        }
    }
}
