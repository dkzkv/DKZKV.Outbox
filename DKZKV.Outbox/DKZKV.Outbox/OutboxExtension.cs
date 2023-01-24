using DKZKV.Outbox.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DKZKV.Outbox;

public static class OutboxExtension
{ 
    public static IServiceCollection AddOutboxProvider(this IServiceCollection services, Action<OutboxDataBuilder> builder)
    {
        var outboxDataBuilder = new OutboxDataBuilder(services);
        builder.Invoke(outboxDataBuilder);
        services.AddScoped<IOutboxProvider, OutboxProvider>();
        services.AddHostedService<MigratorService>();
        return services;
    }

    public static async Task RunOutboxMigrations(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        await scope.ServiceProvider.GetRequiredService<IOutboxRepository>().MigrateAsync(CancellationToken.None);
    }
}