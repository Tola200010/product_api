namespace ProductApi.Entities;
public record Item
{
    public Guid ItemId { get; set;}
    public string? Name { get; set; }
    public double Price { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}