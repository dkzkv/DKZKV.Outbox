namespace DKZKV.Outbox.Abstractions;

public interface IOutboxProvider
{
    Task Publish(OutboxMessage message, CancellationToken token = default);
}