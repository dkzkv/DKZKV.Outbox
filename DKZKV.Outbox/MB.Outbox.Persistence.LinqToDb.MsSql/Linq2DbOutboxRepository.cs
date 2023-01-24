using DKZKV.Outbox.Abstractions;
using LinqToDB;
using LinqToDB.Data;
using MB.Outbox.Persistence.Core.MsSql;

namespace MB.Outbox.Persistence.LinqToDb.MsSql;

internal class Linq2DbOutboxRepository<TDataConnection> : BaseMsSqlRepository , IOutboxRepository where TDataConnection : DataConnection
{
    private readonly TDataConnection _dataConnection;
        
    public Linq2DbOutboxRepository(TDataConnection dataConnection)
    {
        _dataConnection = dataConnection;
    }

    public async Task MigrateAsync(CancellationToken token)
    {
        await _dataConnection.ExecuteAsync(TableMigrationScript, token);
        await _dataConnection.ExecuteAsync(TableCdcScript, token);
    }

    public async Task SaveAsync(OutboxMessage message, CancellationToken token)
    {
        var sqlString = @$"{InsertPrefix} (@Id, @AggregateType, @AggregateId, @Type, @Datetime, @Payload)";

        var idParam = new DataParameter("@Id", message.Id, DataType.Guid);
        var aggregateTypeParam = new DataParameter("@AggregateType", message.AggregateType, DataType.VarChar);
        var aggregateIdParam = new DataParameter("@AggregateId", (object)message.AggregateId ?? DBNull.Value, DataType.Binary);
        var typeParam = new DataParameter("@Type", message.Type, DataType.VarChar);
        var datetimeParam = new DataParameter("@Datetime", message.OccuredOnUtc, DataType.DateTime);
        var payloadParam = new DataParameter("@Payload", message.Payload, DataType.Binary);

        await _dataConnection.ExecuteAsync(sqlString, token, new []{idParam, aggregateTypeParam, aggregateIdParam,
            typeParam, datetimeParam, payloadParam});
    }
}