using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ClearBank.DeveloperTest.Data;

public sealed class DataStoreFactory(
    IOptions<DataStoreOptions> options,
    IServiceProvider serviceProvider
) : IDataStoreFactory
{
    public IAccountDataStore Create()
        => serviceProvider.GetRequiredKeyedService<IAccountDataStore>(
            options.Value.DataStoreType.ToString());
}

public class DataStoreOptions
{
    public DataStoreType DataStoreType { get; init; } = DataStoreType.Normal;
}

public enum DataStoreType
{
    Normal,
    Backup
}