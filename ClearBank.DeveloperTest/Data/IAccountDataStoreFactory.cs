namespace ClearBank.DeveloperTest.Data
{
    public interface IAccountDataStoreFactory
    {
        IAccountDataStore MakeDataStore(string dataStoreType);
    }
}