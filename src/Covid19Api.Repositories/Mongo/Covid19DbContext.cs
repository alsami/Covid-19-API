using System;
using MongoDB.Driver;

namespace Covid19Api.Repositories.Mongo
{
    public class Covid19DbContext
    {
        public Covid19DbContext(Func<IMongoDatabase> databaseComposer)
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