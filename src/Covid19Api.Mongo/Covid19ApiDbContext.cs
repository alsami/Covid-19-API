using System;
using Covid19Api.Mongo.Conventions;
using MongoDB.Driver;

namespace Covid19Api.Mongo
{
    public class Covid19ApiDbContext
    {
        public Covid19ApiDbContext(Func<IMongoDatabase> databaseComposer)
        {
            this.Database = databaseComposer();
            Configure();
        }

        public IMongoDatabase Database { get; }

        private static void Configure()
        {
            MongoDbConventions.UseGuidIdConvetion();
            MongoDbConventions.UseImmutableConvention();
            MongoDbConventions.UseCamelCaseConvention();
            MongoDbConventions.UseIgnoreNullValuesConvention();
            MongoDbConventions.UseEnumStringRepresentation();
        }
    }
}