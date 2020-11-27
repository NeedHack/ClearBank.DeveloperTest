using AutoFixture.Xunit2;
using ClearBank.DeveloperTest.Data;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class AccountDataStoreFactoryTests
    {
        [Fact]
        public void Should_return_BackupAccountDataStore()
        {
            var factory = new AccountDataStoreFactory();
            var dataStore = factory.MakeDataStore("Backup");
            dataStore.Should().BeOfType<BackupAccountDataStore>();
        }

        [Theory, AutoData]
        public void Should_return_AccountDataStore(string dataStoreType)
        {
            var factory = new AccountDataStoreFactory();
            var dataStore = factory.MakeDataStore(dataStoreType);
            dataStore.Should().BeOfType<AccountDataStore>();
        }
    }
}