namespace DKZKV.Outbox.Abstractions;

public interface IOutboxRepository
{
    Task MigrateAsync(CancellationToken token);
    Task SaveAsync(OutboxMessage message, CancellationToken token);
}