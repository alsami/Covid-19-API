using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Covid19Api.Mongo.Migrator.Abstractions
{
    public abstract class DatabaseMigration
    {
        private readonly ILogger logger;

        protected DatabaseMigration(ILogger logger)
        {
            this.logger = logger;
        }
        public abstract int Number { get; }
        
        protected abstract string Name { get; }
        
        public async Task ExecuteUpdateAsync()
        {
            this.logger.LogInformation("Executing migration {number}-{migration}", this.Number, this.Name);
            await ExecuteAsync();
            this.logger.LogInformation("Executed migration {number}-{migration}", this.Number, this.Name);
        }

        protected abstract Task ExecuteAsync();
    }
}