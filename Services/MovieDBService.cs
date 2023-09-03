using MongoExample.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MongoExample.Services
{
    public class MovieDBService
    {
        private readonly IMongoCollection<Movies> _moviesCollection;

        public MovieDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DataBaseName);
            _moviesCollection = database.GetCollection<Movies>(mongoDBSettings.Value.CollectionName);
        }

        public async Task CreateAsync(Movies movies)
        {
            await _moviesCollection.InsertOneAsync(movies);
            return;
        }

        public async Task<List<Movies>> GetAsync()
        {
            return await _moviesCollection.Find(new BsonDocument()).ToListAsync();
        }

    
    }
}