using MongoExample.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MongoExample.Services
{
	public class MongoDBService
	{
		private readonly IMongoCollection<Playlist> _playListCollection;
        private readonly IMongoCollection<Movies> _moviesCollection;

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
		{
			MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
			IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DataBaseName);
			_playListCollection = database.GetCollection<Playlist>(mongoDBSettings.Value.CollectionName);
			_moviesCollection = database.GetCollection<Movies>(mongoDBSettings.Value.CollectionName);
		}

		public async Task CreateAsync(Playlist playlist)
		{
			await _playListCollection.InsertOneAsync(playlist);
			return;
		}

		public async Task<List<Playlist>> GetAsync()
		{
			return await _playListCollection.Find(new BsonDocument()).ToListAsync();
		}

		public async Task AddToPlaylistAsync(string id, string movieId)
		{
			FilterDefinition<Playlist> filter = Builders<Playlist>.Filter.Eq("Id", id);
			UpdateDefinition<Playlist> update = Builders<Playlist>.Update.AddToSet<string>("movieId", movieId);
			await _playListCollection.UpdateOneAsync(filter, update);
		}

		public async Task DeleteAsync(string id)
		{
            FilterDefinition<Playlist> filter = Builders<Playlist>.Filter.Eq("Id", id);
			await _playListCollection.DeleteOneAsync(filter);
			return;
        }
    }
}

