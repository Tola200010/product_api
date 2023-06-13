using ProductApi.Entities;

namespace ProductApi.Repositories.Contracts;
public interface IItemRepository
{
    Task<Item?> GetAsyn(Guid id);
    Task<IEnumerable<Item>> GetAllAsync();
    Task CreateItemAsync(Item item);
    Task UpdateAsync(Item item);
    Task DeleteAsync(Guid id);
}