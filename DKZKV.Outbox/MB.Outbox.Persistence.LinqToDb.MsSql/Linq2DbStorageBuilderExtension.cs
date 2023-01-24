using DKZKV.Outbox.Abstractions;
using LinqToDB.Data;
using Microsoft.Extensions.DependencyInjection;

namespace MB.Outbox.Persistence.LinqToDb.MsSql;

public static class Linq2DbStorageBuilderExtension
{
    public static void UseSqlServer<TDataConnection>(this Linq2DbStorageBuilder<TDataConnection> builder)
        where TDataConnection : DataConnection
    {
        builder.Services.AddScoped<Linq2DbOutboxRepository<TDataConnection>>();
        builder.Services.AddScoped<IOutboxRepository>(provider => provider.GetService<Linq2DbOutboxRepository<TDataConnection>>());
    }
}