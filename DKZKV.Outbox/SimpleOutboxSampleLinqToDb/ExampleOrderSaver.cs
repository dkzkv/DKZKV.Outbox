using System.Diagnostics;
using DKZKV.Outbox.Abstractions;
using LinqToDB;
using SimpleOutboxSampleLinqToDb.Persistence;
using SimpleOutboxSampleLinqToDb.Persistence.Entities;

namespace SimpleOutboxSampleLinqToDb;

public class ExampleOrderSaver : BackgroundService
{
    private readonly ILogger<ExampleOrderSaver> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public ExampleOrderSaver(IServiceScopeFactory scopeFactory, ILogger<ExampleOrderSaver> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        MigrateDatabaseForApplication();
            
        await CreateOrderAndSaveWithPublishing(stoppingToken);
            
        await CreateOrderAndSaveWithPublishing(stoppingToken, true);
    }

    private void MigrateDatabaseForApplication()
    {
        using var scope = _scopeFactory.CreateScope();
        var connection = scope.ServiceProvider.GetRequiredService<AppDataConnection>();
        var sp = connection.DataProvider.GetSchemaProvider();
        var dbSchema = sp.GetSchema(connection);
        if(dbSchema.Tables.All(t => t.TableName != "orders"))
        {
            connection.CreateTable<Order>();
        }
    }
        
    private async Task CreateOrderAndSaveWithPublishing(CancellationToken token, bool isThrowException = false)
    {
        var order = GenerateOrder();
            
        using var scope = _scopeFactory.CreateScope();
        var dataConnection = scope.ServiceProvider.GetService<AppDataConnection>();
        var outboxProvider = scope.ServiceProvider.GetService<IOutboxProvider>() ?? throw new UnreachableException();

        await using var transaction = await dataConnection!.BeginTransactionAsync(token);
        try
        {
            await outboxProvider.Publish(new OutboxMessage(order.Id,
                "Order",
                order.Id.ToByteArray(),
                EventType.Created,
                Utf8Json.JsonSerializer.Serialize(order)
            ), token);

            await dataConnection!.InsertAsync(order, token: token);

            if (isThrowException)
                throw new Exception("Something went wrong, why not...");
                
            await transaction.CommitAsync(token);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(token);
            _logger.LogError(e, "Error while saving order, no data has been persisted, and no messages sent");
        }
    }

    private Order GenerateOrder()
    {
        return new Order()
        {
            Id = Guid.NewGuid(),
            ClientId = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.Now,
            Market = "Amazon",
            Product = "Pen",
            Count = 1
        };
    }
}