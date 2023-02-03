using LinqToDB.Mapping;

namespace SimpleOutboxSampleLinqToDb.Persistence.Entities;

[Table(Name = "orders")]
public class Order 
{
    [PrimaryKey]
    public Guid Id { get; set; }
        
    [Column]
    public Guid ClientId { get; set; }
        
    [Column]
    public DateTimeOffset CreatedAt { get; set; }
        
    [Column]
    public string Market { get; set; }
        
    [Column, NotNull]
    public string Product { get; set; }
        
    [Column]
    public int Count { get; set; }
}