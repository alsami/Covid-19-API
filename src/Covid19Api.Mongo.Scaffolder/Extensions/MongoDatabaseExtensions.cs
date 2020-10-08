using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Covid19Api.Mongo.Scaffolder.Extensions
{
    internal static class MongoDatabaseExtensions
    {
        public static async Task CreateCollectionIfNotExistsAsync(this IMongoDatabase database, string collectionName)
        {
            if (await CollectionExistsAsync(database, collectionName)) return;

            await database.CreateCollectionAsync(collectionName);
        }

        private static async Task<bool> CollectionExistsAsync(this IMongoDatabase database, string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);

            var collections = await database.ListCollectionsAsync(new ListCollectionsOptions
            {
                Filter = filter
            });

            return await collections.AnyAsync();
        }
    }
}