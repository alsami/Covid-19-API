using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Covid19Api.Repositories;
using Covid19Api.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Covid19Api.Worker
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
                    await this.DoStuffAsync();

                    await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
                }
                catch (Exception e)
                {
                    this.logger.LogCritical(e, e.Message);
                }
            }
        }

        private async Task DoStuffAsync()
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

            await latestStatsRepo.AddAsync(latestStats);

            var activeCasesStatsRepo = scope.Resolve<ActiveCasesRepository>();

            await activeCasesStatsRepo.AddAsync(activeCaseStats);

            var closedCasesStatsRepo = scope.Resolve<ClosedCasesRepository>();

            await closedCasesStatsRepo.AddAsync(closedCasesStats);

            var countryStatsRepository = scope.Resolve<CountryStatsRepository>();

            await countryStatsRepository.AddManyAsync(countryStats);
        }
    }
}