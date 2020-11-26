using ClearBank.DeveloperTest.Data;

namespace ClearBank.DeveloperTest.Services
{
    public class AccountDataStoreFactory : IAccountDataStoreFactory
    {
        public IAccountDataStore MakeDataStore(string dataStoreType)
        {
            if (dataStoreType == "Backup")
            {
                return new BackupAccountDataStore();
            }

            return new AccountDataStore();
        }
    }
}