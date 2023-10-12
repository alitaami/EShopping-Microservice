using MongoDB.Bson;
using MongoDB.Driver;

public class Repository<T> : IRepository<T> where T : class
{
     
    private readonly IMongoCollection<T> _collection;
    private readonly IMongoDatabase _database;

    public Repository(IMongoDatabase database)
    {
        _database = database;
        
        var collectionName = typeof(T).Name + "s"; // Assumes a convention like Product -> Products
        _collection = _database.GetCollection<T>(collectionName);
    }


    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _collection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task<T> GetByIdAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq("Id", id);
        return await _collection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(string id, T entity)
    {
        var filter = Builders<T>.Filter.Eq("Id", id);
        await _collection.ReplaceOneAsync(filter, entity);
    }

    public async Task DeleteAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq("Id", id);
        await _collection.DeleteOneAsync(filter);
    }
}
