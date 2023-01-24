namespace DKZKV.Outbox.Abstractions;

public class OutboxMessage
{
    public static short AggregateTypeLength => 256;
    public static short TypeLength => 256;

    public OutboxMessage(Guid id, string aggregateType, EventType type, byte[] payload)
    {
        Validate(id, aggregateType, null, type.ToString(), payload);
        Id = id;
        AggregateType = aggregateType;
        Type = type.ToString();
        OccuredOnUtc = DateTime.UtcNow;
        Payload = payload;
    }

    public OutboxMessage(Guid id, string aggregateType, byte[] aggregateId, EventType type, byte[] payload)
    {
        Validate(id, aggregateType, aggregateId, type.ToString(), payload);
        Id = id;
        AggregateType = aggregateType;
        AggregateId = aggregateId;
        Type = type.ToString();
        OccuredOnUtc = DateTime.UtcNow;
        Payload = payload;
    }

    public OutboxMessage(Guid id, string aggregateType, byte[] aggregateId, string type, byte[] payload)
    {
        Validate(id, aggregateType, aggregateId, type, payload);
        Id = id;
        AggregateType = aggregateType;
        AggregateId = aggregateId;
        Type = type;
        OccuredOnUtc = DateTime.UtcNow;
        Payload = payload;
    }
        
    public OutboxMessage(Guid id, string aggregateType, string type, byte[] payload)
    {
        Validate(id, aggregateType, null, type, payload);
        Id = id;
        AggregateType = aggregateType;
        Type = type;
        OccuredOnUtc = DateTime.UtcNow;
        Payload = payload;
    }

    private void Validate(Guid id, string aggregateType, byte[]? aggregateId, string type, byte[] payload)
    {
        if (id == Guid.Empty)
            throw new ArgumentException($"{nameof(id)} could not be empty");
            
        if(string.IsNullOrEmpty(aggregateType) || aggregateType.Length > AggregateTypeLength)
            throw new ArgumentException($"{nameof(aggregateType)} should not be empty and greater than {AggregateTypeLength}");
            
        if(string.IsNullOrEmpty(type) || type.Length > TypeLength)
            throw new ArgumentException($"{nameof(type)} should not be empty and greater than {TypeLength}");
            
        if(payload.Length == 0)
            throw new ArgumentException($"{nameof(payload)} should not be empty");

        if (aggregateId is not null)
        {
            if(aggregateId.Length == 0)
                throw new ArgumentException($"{nameof(aggregateId)} should not be empty");
        }
    }
        
    public Guid Id { get; }
    public string AggregateType { get; }
#nullable enable
    public byte[]? AggregateId { get; }
#nullable disable
    public string Type { get; }
    public DateTime OccuredOnUtc { get; }
    public byte[] Payload { get; }
}