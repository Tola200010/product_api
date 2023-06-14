using ProductApi.Entities;

namespace ProductApi.Repositories.Contracts;
public interface IItemRepository
{
    Task<Item?> GetAsync(Guid id);
    Task<IEnumerable<Item>> GetAllAsync();
    Task<Item> CreateItemAsync(Item item);
    Task<bool> UpdateAsync(Item item);
    Task<bool> DeleteAsync(Guid id);
}