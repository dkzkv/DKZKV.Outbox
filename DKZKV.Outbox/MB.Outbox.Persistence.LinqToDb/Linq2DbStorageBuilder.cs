using LinqToDB.Data;
using Microsoft.Extensions.DependencyInjection;

namespace MB.Outbox.Persistence.LinqToDb;

public class Linq2DbStorageBuilder<TDataConnection> where TDataConnection : DataConnection
{
    public Linq2DbStorageBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public IServiceCollection Services { get; }
}