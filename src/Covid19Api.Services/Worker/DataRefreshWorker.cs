using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Covid19Api.Domain;
using Covid19Api.Repositories;
using Covid19Api.Services.Parser;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Covid19Api.Services.Worker
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DataRefreshWorker : BackgroundService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<DataRefreshWorker> logger;

        public DataRefreshWorker(IHttpClientFactory httpClientFactory, IServiceProvider serviceProvider, ILogger<DataRefreshWorker> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await this.ProcessAsync();
                }
                catch (Exception e)
                {
                    this.logger.LogCritical(e, e.Message);
                }
                
                await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
            }
        }

        private async Task ProcessAsync()
        {
            this.logger.LogInformation("Start fetching html document");

            var fetchedAt = DateTime.UtcNow;

            var client = this.httpClientFactory.CreateClient();

            var document = await HtmlDocumentFetcher.FetchAsync(client);

            var latestStats = LatestStatsParser.Parse(document, fetchedAt);

            var activeCaseStats = ActiveCasesParser.Parse(document, fetchedAt);

            var closedCasesStats = ClosedCasesParser.Parse(document, fetchedAt);

            var countryStats = CountryStatsParser.Parse(document, fetchedAt);

            this.logger.LogInformation("Storing fetched data");
            
            await using var scope = this.serviceProvider.GetAutofacRoot().BeginLifetimeScope();

            var latestStatsRepo = scope.Resolve<LatestStatsRepository>();

            await latestStatsRepo.StoreAsync(latestStats);

            var activeCasesStatsRepo = scope.Resolve<ActiveCasesStatsRepository>();

            await activeCasesStatsRepo.StoreAsync(activeCaseStats);

            var closedCasesStatsRepo = scope.Resolve<ClosedCasesRepository>();

            await closedCasesStatsRepo.StoreAsync(closedCasesStats);

            var countryStatsRepository = scope.Resolve<CountryStatsRepository>();

            foreach (var chunkedStats in CreateChunks(countryStats.ToList()))
            {
                try
                {
                    await countryStatsRepository.StoreAsync(chunkedStats);
                }
                catch (Exception e)
                {
                    this.logger.LogCritical(e, e.Message);
                }
                
                await Task.Delay(1000);
            }
        }
        
        private static IEnumerable<List<CountryStats>> CreateChunks(List<CountryStats> countryStats, int chunkSize = 50)  
        {        
            for (var i = 0; i < countryStats.Count; i += chunkSize) 
            { 
                yield return countryStats.GetRange(i, Math.Min(chunkSize, countryStats.Count - i)); 
            }  
        } 
    }
}