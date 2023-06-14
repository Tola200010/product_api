
using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.Entities;
using ProductApi.Repositories.Contracts;

namespace ProductApi.Repositories;
public class ItemRepository : IItemRepository
{
	private readonly DataContext _dataContext;

	public ItemRepository(DataContext dataContext)
	{
		_dataContext = dataContext;
	}
	public async Task<Item?> GetAsync(Guid id)
	{
		return await _dataContext.Items!.FindAsync(id);
	}

	public async Task<IEnumerable<Item>> GetAllAsync()
	{
		var items = await _dataContext.Items!.ToListAsync();
		return items;
	}

	public async Task<Item> CreateItemAsync(Item item)
	{
		await _dataContext.Items!.AddAsync(item);
		await _dataContext.SaveChangesAsync();
		return item;
	}

	public async Task<bool> UpdateAsync(Item item)
	{
		 _dataContext.Items!.Update(item);
		 var result = await _dataContext.SaveChangesAsync();
		 return result > 0;
	}

	public async Task<bool> DeleteAsync(Guid id)
	{
		 var item =await _dataContext.Items!.FindAsync(id);
		 if (item == null)
		 {
			 return false;
		 }
		 _dataContext.Items!.Remove(item);
		 var result = await _dataContext.SaveChangesAsync();
		 return result > 0;
	}
}