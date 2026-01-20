using System;
using ClearBank.DeveloperTest.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class DataStoreFactoryTests
    {
        private ServiceCollection _services;

        public DataStoreFactoryTests()
        {
            _services = new ServiceCollection();

            _services.AddKeyedTransient<IAccountDataStore, AccountDataStore>(DataStoreType.Normal.ToString());
            _services.AddKeyedTransient<IAccountDataStore, BackupAccountDataStore>(DataStoreType.Backup.ToString());
            _services.AddTransient<IDataStoreFactory, DataStoreFactory>();
        }

        [Fact]
        public void Create_WhenDataStoreType_IsNormal_Returns_NormalStore()
        {
            _services.AddSingleton(
                Options.Create(new DataStoreOptions { DataStoreType = DataStoreType.Normal }));

            using var sp = _services.BuildServiceProvider();
            var sut = sp.GetRequiredService<IDataStoreFactory>();

            var store = sut.Create();

            Assert.IsType<AccountDataStore>(store);
        }

        [Fact]
        public void Create_WhenDataStoreType_IsBackup_Returns_BackupStore()
        {
            _services.AddSingleton(
                Options.Create(new DataStoreOptions { DataStoreType = DataStoreType.Backup }));

            using var sp = _services.BuildServiceProvider();
            var sut = sp.GetRequiredService<IDataStoreFactory>();

            var store = sut.Create();

            Assert.IsType<BackupAccountDataStore>(store);
        }
    }
}
