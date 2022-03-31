using Microsoft.Extensions.Logging;

namespace Covid19Api.Mongo.Scaffolder.Abstractions;

public abstract class DatabaseUpdateDefinition
{
    private readonly ILogger logger;

    protected DatabaseUpdateDefinition(ILogger logger)
    {
        this.logger = logger;
    }

    public abstract int Version { get; }

    public async Task ExecuteUpdateAsync()
    {
        this.logger.LogInformation("Updating Schema to Version {Version}", Version);
        await ExecuteAsync();
        this.logger.LogInformation("Schema has been updated to Version {Version}", Version);
    }

    protected abstract Task ExecuteAsync();
}