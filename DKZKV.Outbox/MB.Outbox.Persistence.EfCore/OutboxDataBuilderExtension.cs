using DKZKV.Outbox.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MB.Outbox.Persistence.EfCore;

public static class OutboxDataBuilderExtension
{
    public static void UseEfCore<TDbContext>(this OutboxDataBuilder builder, Action<EfCoreStorageBuilder<TDbContext>> storageBuilder) 
        where TDbContext : DbContext
    {
        var efCoreBuilder = new EfCoreStorageBuilder<TDbContext>(builder.Services);
        storageBuilder.Invoke(efCoreBuilder);
    }
        
}