using Covid19Api.Domain;
using Covid19Api.Mongo.Conventions;
using MongoDB.Driver;

namespace Covid19Api.Mongo;

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
        MongoDbConventions.UseGuidIdConvention(typeof(VaccinationStatistic));
        MongoDbConventions.IgnoreNotMappedPropertiesConvention(typeof(VaccinationStatistic));
        MongoDbConventions.UseImmutableConvention();
        MongoDbConventions.UseCamelCaseConvention();
        MongoDbConventions.UseIgnoreNullValuesConvention();
        MongoDbConventions.UseEnumStringRepresentation();
    }
}