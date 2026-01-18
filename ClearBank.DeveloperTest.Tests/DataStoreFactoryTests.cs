using System;
using ClearBank.DeveloperTest.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class DataStoreFactoryTests
    {
        [Fact]
        public void Create_WhenDataStoreTypeIsNormal_ReturnsNormalKeyedStore()
        {
            // Arrange
            var services = new ServiceCollection();

            services.AddKeyedTransient<IAccountDataStore, AccountDataStore>(DataStoreType.Normal.ToString());
            services.AddKeyedTransient<IAccountDataStore, BackupAccountDataStore>(DataStoreType.Backup.ToString());

            services.AddSingleton(
                Options.Create(new DataStoreOptions { DataStoreType = DataStoreType.Normal }));

            services.AddTransient<IDataStoreFactory, DataStoreFactory>();

            using var sp = services.BuildServiceProvider();
            var sut = sp.GetRequiredService<IDataStoreFactory>();

            // Act
            var store = sut.Create();

            // Assert
            Assert.IsType<AccountDataStore>(store);
        }

        [Fact]
        public void Create_WhenDataStoreTypeIsBackup_ReturnsBackupKeyedStore()
        {
            // Arrange
            var services = new ServiceCollection();

            services.AddKeyedTransient<IAccountDataStore, AccountDataStore>(DataStoreType.Normal.ToString());
            services.AddKeyedTransient<IAccountDataStore, BackupAccountDataStore>(DataStoreType.Backup.ToString());

            services.AddSingleton<IOptions<DataStoreOptions>>(
                Options.Create(new DataStoreOptions { DataStoreType = DataStoreType.Backup }));

            services.AddTransient<IDataStoreFactory, DataStoreFactory>();

            using var sp = services.BuildServiceProvider();
            var sut = sp.GetRequiredService<IDataStoreFactory>();

            // Act
            var store = sut.Create();

            // Assert
            Assert.IsType<BackupAccountDataStore>(store);
        }

        [Fact]
        public void Create_WhenKeyedServiceNotRegistered_ThrowsInvalidOperationException()
        {
            // Arrange
            var services = new ServiceCollection();

            // Only register Normal, but options request Backup
            services.AddKeyedTransient<IAccountDataStore, AccountDataStore>(DataStoreType.Normal.ToString());

            services.AddSingleton<IOptions<DataStoreOptions>>(
                Options.Create(new DataStoreOptions { DataStoreType = DataStoreType.Backup }));

            services.AddTransient<IDataStoreFactory, DataStoreFactory>();

            using var sp = services.BuildServiceProvider();
            var sut = sp.GetRequiredService<IDataStoreFactory>();

            // Act + Assert
            Assert.Throws<InvalidOperationException>(() => sut.Create());
        }


    }
}
