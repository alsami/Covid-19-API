using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;

namespace Covid19Api.Repositories.Mongo
{
    internal static class MongoDbConventions
    {
        public static void UseGuidIdConvetion()
        {
            ConventionRegistry.Register(nameof(GuidIdConvention), new ConventionPack
            {
                new GuidIdConvention()
            }, _ => true);
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
            ConventionRegistry.Register(nameof(IgnoreIfDefaultConvention),
                new ConventionPack {new IgnoreIfDefaultConvention(true)}, _ => true);
        }

        public static void UseEnumStringRepresentation()
        {
            ConventionRegistry.Register(nameof(EnumRepresentationConvention), new ConventionPack
            {
                new EnumRepresentationConvention(BsonType.String)
            }, _ => true);
        }
    }
}