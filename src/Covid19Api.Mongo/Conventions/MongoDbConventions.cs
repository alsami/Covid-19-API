using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;

namespace Covid19Api.Mongo.Conventions;

internal static class MongoDbConventions
{
    public static void UseGuidIdConvention(params Type[] typesToExclude)
    {
        ConventionRegistry.Register(nameof(GuidIdConvention), new ConventionPack
        {
            new GuidIdConvention()
        }, type => !typesToExclude.Contains(type));
    }

    public static void IgnoreNotMappedPropertiesConvention(params Type[] typesToIgnoreExtraElementsFor)
    {
        ConventionRegistry.Register(nameof(IgnoreExtraElementsConvention), new ConventionPack
        {
            new IgnoreExtraElementsConvention(true)
            // ReSharper disable once ConvertClosureToMethodGroup
        }, type => typesToIgnoreExtraElementsFor.Contains(type));
    }

    public static void UseImmutableConvention()
    {
        ConventionRegistry.Register(nameof(ImmutableTypeClassMapConvention), new ConventionPack
        {
            new ImmutableTypeClassMapConvention()
        }, type => true);
    }

    public static void UseCamelCaseConvention()
    {
        ConventionRegistry.Register(nameof(CamelCaseElementNameConvention),
            new ConventionPack {new CamelCaseElementNameConvention()},
            type => true);
    }

    public static void UseIgnoreNullValuesConvention()
    {
        ConventionRegistry.Register(nameof(IgnoreIfNullConvention),
            new ConventionPack {new IgnoreIfNullConvention(true)}, _ => true);
    }

    public static void UseEnumStringRepresentation()
    {
        ConventionRegistry.Register(nameof(EnumRepresentationConvention), new ConventionPack
        {
            new EnumRepresentationConvention(BsonType.String)
        }, _ => true);
    }
}