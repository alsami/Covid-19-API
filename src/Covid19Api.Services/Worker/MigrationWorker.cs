using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Covid19Api.Domain;
using Covid19Api.Repositories.Mongo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Covid19Api.Services.Worker
{
    public class MigrationWorker : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private const string CountriesCollectionName = "countrystats";
        private const string LatestStatsCollectionName = "lateststats";
        private static readonly DateTime Tommorow = DateTime.UtcNow.Date.AddDays(1);
        private readonly ILogger<MigrationWorker> logger;

        public MigrationWorker(IServiceProvider serviceProvider, ILogger<MigrationWorker> logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = this.serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<Covid19DbContext>();


            static IMongoDatabase LocalDatabaseComposer()
            {
                var settings = MongoClientSettings.FromUrl(new MongoUrl("mongodb://admin:admin@localhost:27017"));

                var client = new MongoClient(settings);

                return client.GetDatabase("Covid19");
            }

            var localContext = new Covid19DbContext(LocalDatabaseComposer);

            await this.MigrateGlobalStats(stoppingToken, localContext);
        }

        private async Task CopyCountriesToLocal(CancellationToken stoppingToken, Covid19DbContext remoteContext,
            Covid19DbContext localContext)
        {
            var remoteCountriesCollection = remoteContext.Database.GetCollection<CountryStats>(CountriesCollectionName);

            var cursor = await remoteCountriesCollection.DistinctAsync<string>("country", new BsonDocument(),
                cancellationToken: stoppingToken);

            var countries = await cursor.ToListAsync(stoppingToken);

            await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);

            foreach (var country in countries)
            {
                try
                {
                    var migrationDate = new DateTime(2020, 3, 15);
                    while (migrationDate < Tommorow)
                    {
                        var migratioNDatePlusOne = migrationDate.AddDays(+1);

                        var sort = Builders<CountryStats>.Sort
                            .Descending("FetchedAt");

                        // ReSharper disable once SpecifyStringComparison
                        var searchCursor = await remoteCountriesCollection.FindAsync(
                            existingCountryStats =>
                                existingCountryStats.Country.ToLower() == country.ToLower() &&
                                migratioNDatePlusOne > existingCountryStats.FetchedAt &&
                                existingCountryStats.FetchedAt >= migrationDate,
                            new FindOptions<CountryStats>
                            {
                                Sort = sort,
                            }, stoppingToken);

                        var list = await searchCursor.ToListAsync();

                        if (!list.Any())
                        {
                            migrationDate = migrationDate.AddDays(1);
                            await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                            continue;
                        }

                        this.logger.LogInformation("Inserting {n} elements to local database for {country} on {date}",
                            list.Count, country, migrationDate.ToString("O"));

                        await localContext.Database.GetCollection<CountryStats>(CountriesCollectionName)
                            .InsertManyAsync(list, cancellationToken: stoppingToken);

                        migrationDate = migrationDate.AddDays(1);
                        await Task.Delay(1000, stoppingToken);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        private async Task MigrateGlobalStats(CancellationToken stoppingToken, Covid19DbContext context)
        {
            var globalStatsCollection = context.Database.GetCollection<GlobalStats>(LatestStatsCollectionName);

            try
            {
                // await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                var migrationDate = new DateTime(2020, 3, 15);
                while (migrationDate < Tommorow)
                {
                    var migratioNDatePlusOne = migrationDate.AddDays(+1);

                    var sort = Builders<GlobalStats>.Sort
                        .Descending("FetchedAt");

                    // ReSharper disable once SpecifyStringComparison
                    var searchCursor = await globalStatsCollection.FindAsync(
                        existingCountryStats => migratioNDatePlusOne > existingCountryStats.FetchedAt &&
                                                existingCountryStats.FetchedAt >= migrationDate,
                        new FindOptions<GlobalStats>
                        {
                            Sort = sort,
                        }, stoppingToken);

                    var list = await searchCursor.ToListAsync(cancellationToken: stoppingToken);

                    if (!list.Any())
                    {
                        migrationDate = migrationDate.AddDays(1);
                        // await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                        continue;
                    }

                    var toRemoveList = list.ToList();
                    toRemoveList.RemoveAt(0);

                    if (!toRemoveList.Any())
                    {
                        migrationDate = migrationDate.AddDays(1);
                        // await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                        continue;
                    }

                    this.logger.LogInformation(
                        "WOULD DELETE {n} of {m} elements for lateststats on date {date}",
                        toRemoveList.Count, list.Count, migrationDate.ToString("O"));

                    foreach (var toRemove in toRemoveList)
                    {
                        await globalStatsCollection.DeleteOneAsync(s => s.Id == toRemove.Id,
                            cancellationToken: stoppingToken);
                    }

                    migrationDate = migrationDate.AddDays(1);
                    // await Task.Delay(2000, stoppingToken);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task MigrateCountries(CancellationToken stoppingToken, Covid19DbContext context)
        {
            var countriesCollection = context.Database.GetCollection<CountryStats>(CountriesCollectionName);

            var cursor = await countriesCollection.DistinctAsync<string>("country", new BsonDocument(),
                cancellationToken: stoppingToken);

            var countries = await cursor.ToListAsync(stoppingToken);

            await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);

            foreach (var country in countries)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                    var migrationDate = new DateTime(2020, 3, 15);
                    while (migrationDate < Tommorow)
                    {
                        var migratioNDatePlusOne = migrationDate.AddDays(+1);

                        var sort = Builders<CountryStats>.Sort
                            .Descending("FetchedAt");

                        // ReSharper disable once SpecifyStringComparison
                        var searchCursor = await countriesCollection.FindAsync(
                            existingCountryStats =>
                                existingCountryStats.Country.ToLower() == country.ToLower() &&
                                migratioNDatePlusOne > existingCountryStats.FetchedAt &&
                                existingCountryStats.FetchedAt >= migrationDate,
                            new FindOptions<CountryStats>
                            {
                                Sort = sort,
                            }, stoppingToken);

                        var list = await searchCursor.ToListAsync();

                        if (!list.Any())
                        {
                            migrationDate = migrationDate.AddDays(1);
                            await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                            continue;
                        }

                        var toRemoveList = list.ToList();
                        toRemoveList.RemoveAt(0);

                        if (!toRemoveList.Any())
                        {
                            migrationDate = migrationDate.AddDays(1);
                            await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                            continue;
                        }

                        this.logger.LogInformation(
                            "WOULD DELETE {n} of {m} elements for country {country} on date {date}",
                            toRemoveList.Count, list.Count, country, migrationDate.ToString("O"));

                        foreach (var toRemove in toRemoveList)
                        {
                            await countriesCollection.DeleteOneAsync(s => s.Id == toRemove.Id);
                            this.logger.LogInformation("Deleting for {country} and date {date}", country,
                                toRemove.FetchedAt.ToString("O"));
                            await Task.Delay(100, stoppingToken);
                        }

                        migrationDate = migrationDate.AddDays(1);
                        await Task.Delay(2000, stoppingToken);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}