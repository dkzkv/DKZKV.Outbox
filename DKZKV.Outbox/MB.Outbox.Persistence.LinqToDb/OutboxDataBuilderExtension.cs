using DKZKV.Outbox.Abstractions;
using LinqToDB.Data;

namespace MB.Outbox.Persistence.LinqToDb;

public static class OutboxDataBuilderExtension
{
    public static void UseLinq2Db<TDataConnection>(this OutboxDataBuilder builder, Action<Linq2DbStorageBuilder<TDataConnection>> storage) 
        where TDataConnection : DataConnection
    {
        var linq2BdBuilder = new Linq2DbStorageBuilder<TDataConnection>(builder.Services);
        storage.Invoke(linq2BdBuilder);
    }
}