# DKZKV.Outbox

A lightweight library that allows you to implement transaction outbox

<img src="https://github.com/dkzkv/DKZKV.Outbox/blob/main/assets/outbox-1.png?raw=true" alt="drawing" width="400"/>

### Supports:
+ Linq2db
    + MsSql
+ EntityFrameworkCore
    + MsSql

### How to use
For EntityFrameworkCore:
```csharp
services.AddDbContext<MyDbContext>(builder => builder.UseSqlServer(MsSqlConnectionString));

//Add this
services.AddOutboxProvider(builder => builder.UseEfCore<MyDbContext>(o => o.UseSqlServer()));
```
or if linq2db is used then choose UseLinq2Db and select necessary provider 

```csharp
services.AddOutboxProvider(builder => builder.UseLinq2Db<AppDataConnection>(o => o.UseSqlServer()));```
```
That is all needed, now just invoke IOutboxProvider in same  same transaction in which you interact with the persistence.

> **IMPORTANT**: IOutboxProvider should be in same *scope* as persistence provider

```csharp
await using var transaction = await dataConnection!.BeginTransactionAsync(token);
try
{
    //Send event through outbox message relay
    await outboxProvider.Publish(new OutboxMessage(order.Id,
        "Order",
        order.Id.ToByteArray(),
        EventType.Created,
        Utf8Json.JsonSerializer.Serialize(order)
        ), token);

    await dataConnection!.InsertAsync(order, token: token);      
    await transaction.CommitAsync(token);
}
catch (Exception e)
...     
```

### About Transaction outbox
Read [this](https://microservices.io/patterns/data/transactional-outbox.html).

> IMPORTANT:  As a message relay [Debezium](https://debezium.io/) is strongly recommended




