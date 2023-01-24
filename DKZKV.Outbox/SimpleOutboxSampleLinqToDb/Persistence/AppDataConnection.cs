using LinqToDB;
using LinqToDB.Configuration;
using LinqToDB.Data;
using SimpleOutboxSampleLinqToDb.Persistence.Entities;

namespace SimpleOutboxSampleLinqToDb.Persistence;

public class AppDataConnection : DataConnection
{
    public AppDataConnection(LinqToDBConnectionOptions<AppDataConnection> options)
        : base(options)
    {
    }

    public ITable<Order> Orders => this.GetTable<Order>();
}