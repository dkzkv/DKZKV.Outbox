using DKZKV.Outbox.Abstractions;

namespace DKZKV.Outbox;

internal class OutboxProvider : IOutboxProvider
{
    private readonly IOutboxRepository _outboxRepository;

    public OutboxProvider(IOutboxRepository outboxRepository)
    {
        _outboxRepository = outboxRepository;
    }

    public Task Publish(OutboxMessage message, CancellationToken token)
    {
        if (message is null)
            throw new ArgumentNullException(nameof(message));
        return _outboxRepository.SaveAsync(message, token);
    }
}