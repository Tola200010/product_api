namespace ProductApi.Entities;
public record Item
{
    public Guid ItemId { get; init;}
    public string? Name { get; init;}
    public double Price { get; init;}
    public DateTimeOffset CreatedDate { get; init;}
}