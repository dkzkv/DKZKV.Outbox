namespace DKZKV.Outbox.Abstractions;

public enum EventType 
{
    Created,   
    Updated,
    Canceled,
    Deleted
}