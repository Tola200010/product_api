using MongoDB.Bson;
using MongoDB.Driver;
using ProductApi.Entities;
using ProductApi.Repositories.Contracts;

namespace ProductApi.Repositories;
public class ItemRepository : IItemRepository
{
    private const string databaseName = "catalog";
    private const string collectionName = "items";
    private readonly IMongoCollection<Item> _itemsCollection;
    private readonly FilterDefinitionBuilder<Item> _filterBuilder = Builders<Item>.Filter;
    public ItemRepository(IMongoClient client){
        var database = client.GetDatabase(databaseName);
        _itemsCollection = database.GetCollection<Item>(collectionName);
    }
    public async Task CreateItemAsync(Item item)
    {
       await _itemsCollection.InsertOneAsync(item);
    }

    public async Task DeleteAsync(Guid id)
    {
        var filter = _filterBuilder.Eq(item=>item.ItemId, id);
        await _itemsCollection.DeleteOneAsync(filter);
    }

    public async Task<IEnumerable<Item>> GetAllAsync()
    {
        return await _itemsCollection.Find(new BsonDocument()).ToListAsync();
    }

    public async  Task<Item?> GetAsyn(Guid id)
    {
        var filter = _filterBuilder.Eq(item=>item.ItemId, id);
        return await _itemsCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(Item item)
    {
        var filter= _filterBuilder.Eq(existsItem => existsItem.ItemId,item.ItemId);
        await _itemsCollection.ReplaceOneAsync(filter,item);
    }
}