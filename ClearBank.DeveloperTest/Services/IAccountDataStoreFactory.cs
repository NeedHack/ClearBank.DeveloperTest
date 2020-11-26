using ClearBank.DeveloperTest.Data;

namespace ClearBank.DeveloperTest.Services
{
    public interface IAccountDataStoreFactory
    {
        IAccountDataStore MakeDataStore(string dataStoreType);
    }
}