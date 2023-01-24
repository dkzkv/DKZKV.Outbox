using DKZKV.Outbox.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MB.Outbox.Persistence.EfCore.MsSql;

public static class EfCoreStorageBuilderExtension
{
    public static void UseSqlServer<TContext>(this EfCoreStorageBuilder<TContext> builder) where TContext : DbContext
    {
        builder.Services.AddScoped<EfCoreRepository<TContext>>();
        builder.Services.AddScoped<IOutboxRepository>(provider => provider.GetService<EfCoreRepository<TContext>>());
    }
}