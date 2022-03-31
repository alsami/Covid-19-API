using Covid19Api.Domain;
using Covid19Api.Mongo;
using Covid19Api.Repositories.Abstractions;
using MongoDB.Driver;

namespace Covid19Api.Repositories;

public class RequestLogWriteRepository : IRequestLogWriteRepository
{
    private readonly Covid19ApiDbContext context;

    public RequestLogWriteRepository(Covid19ApiDbContext context)
    {
        this.context = context;
    }

    public Task StoreAsync(RequestLog requestLog)
    {
        var collection = this.GetCollection();

        return collection.InsertOneAsync(requestLog);
    }

    private IMongoCollection<RequestLog> GetCollection()
        => this.context.Database.GetCollection<RequestLog>(CollectionNames.RequestLogs);
}