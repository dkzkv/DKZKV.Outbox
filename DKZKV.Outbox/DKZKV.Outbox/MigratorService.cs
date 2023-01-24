using DKZKV.Outbox.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DKZKV.Outbox;

internal class MigratorService : IHostedService
{
    private readonly IServiceScopeFactory _factory;
        
    public MigratorService(IServiceScopeFactory factory)
    {
        _factory = factory;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _factory.CreateAsyncScope();
        var outboxRepository = scope.ServiceProvider.GetService<IOutboxRepository>();
        if (outboxRepository is null)
            throw new InvalidOperationException($"{nameof(IOutboxRepository)} implementation is not defined");
            
        await outboxRepository.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}