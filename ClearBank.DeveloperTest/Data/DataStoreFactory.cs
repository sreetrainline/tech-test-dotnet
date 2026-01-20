using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ClearBank.DeveloperTest.Data;

public class DataStoreFactory(
    IOptions<DataStoreOptions> options,
    IServiceProvider serviceProvider
) : IDataStoreFactory
{
    public IAccountDataStore Create()
        => serviceProvider.GetRequiredKeyedService<IAccountDataStore>(
            options.Value.DataStoreType.ToString());
}