using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Covid19Api.Mongo.Conventions;

internal class GuidIdConvention : ConventionBase, IPostProcessingConvention
{
    public void PostProcess(BsonClassMap classMap)
    {
        var idMap = classMap.IdMemberMap;
        if (idMap == null || idMap.MemberName != "Id" || idMap.MemberType != typeof(Guid)) return;

        idMap.SetIdGenerator(new GuidGenerator());
    }
}