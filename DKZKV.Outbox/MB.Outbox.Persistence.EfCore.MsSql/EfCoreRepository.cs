using System.Data;
using DKZKV.Outbox.Abstractions;
using MB.Outbox.Persistence.Core.MsSql;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MB.Outbox.Persistence.EfCore.MsSql;

internal class EfCoreRepository<TContext> : BaseMsSqlRepository, IOutboxRepository where TContext : DbContext
{
    private readonly TContext _ctx;
    public EfCoreRepository(TContext ctx)
    {
        _ctx = ctx;
    }
	
    public async Task MigrateAsync(CancellationToken token)
    {
        await _ctx.Database.ExecuteSqlRawAsync(TableMigrationScript, token);
        await _ctx.Database.ExecuteSqlRawAsync(TableCdcScript, token);
    }
	
    public async Task SaveAsync(OutboxMessage message, CancellationToken token)
    {
        var sqlString = $"{InsertPrefix} (@Id, @AggregateType, @AggregateId, @Type, @Datetime, @Payload)";
			
        var idParam = new SqlParameter("@AggregateId", message.Id);
        idParam.DbType = DbType.Guid;
			
        var aggregateTypeParam = new SqlParameter("@AggregateType", message.AggregateType);
        aggregateTypeParam.DbType = DbType.String;
			
        var aggregateIdParam = new SqlParameter("@AggregateId", (object)message.AggregateId! ?? DBNull.Value);
        aggregateIdParam.DbType = DbType.Binary;
			
        var typeParam = new SqlParameter("@Type", message.Type);
        typeParam.DbType = DbType.String;
			
        var dateTimeParam = new SqlParameter("@Datetime", message.Type);
        dateTimeParam.DbType = DbType.DateTime;
			
        var payloadParam = new SqlParameter("@Payload", message.Payload);
        payloadParam.DbType = DbType.Binary;
        await _ctx.Database.ExecuteSqlRawAsync(sqlString, new[] {idParam, aggregateTypeParam, aggregateIdParam, typeParam,
            dateTimeParam, payloadParam});
    }
}