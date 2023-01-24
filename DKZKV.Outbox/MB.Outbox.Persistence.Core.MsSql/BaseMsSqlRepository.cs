using DKZKV.Outbox.Abstractions;

namespace MB.Outbox.Persistence.Core.MsSql;

public abstract class BaseMsSqlRepository
{
    public readonly string TableMigrationScript = @$"IF NOT EXISTS (SELECT * FROM sys.objects 
                     WHERE object_id = OBJECT_ID(N'[outbox].[outbox_messages]') AND type in (N'U'))
                     BEGIN
	                     EXEC ('CREATE SCHEMA [outbox]')
                         CREATE TABLE [outbox].[outbox_messages]
                         (
	                        id uniqueidentifier PRIMARY KEY NOT NULL,
                            aggregatetype varchar({OutboxMessage.AggregateTypeLength}) NOT NULL,
                            aggregateid varbinary(max) NULL,
                            type varchar({OutboxMessage.TypeLength}) NOT NULL,
                            occuredonutc datetime NOT NULL,
                            payload varbinary(max) NOT NULL
                         )
                     END";

    public readonly string TableCdcScript = @"IF NOT EXISTS (select is_tracked_by_cdc 
                 from (select s.name + '.' + t.name fullname, 
                 t.is_tracked_by_cdc
                 from sys.tables t, sys.schemas s
                  where t.schema_id = s.schema_id) i
                 where fullname = 'outbox.outbox_messages' and is_tracked_by_cdc=1)
                 BEGIN
                  EXEC sys.sp_cdc_enable_table
                   @source_schema = N'outbox',
                   @source_name   = N'outbox_messages', 
                   @role_name     = NULL,  
                   @filegroup_name = N'PRIMARY',
                   @supports_net_changes = 0
                 END";

    public readonly string InsertPrefix = "INSERT INTO [outbox].[outbox_messages] (id, aggregatetype, aggregateid, type, occuredonutc, payload) VALUES";
}