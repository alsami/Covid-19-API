using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Covid19Api.Services;
using Microsoft.Extensions.Hosting;

namespace Covid19Api.Worker
{
    public class DataRefreshWorker : BackgroundService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public DataRefreshWorker(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var fetchedAt = DateTime.UtcNow;

            var client = this.httpClientFactory.CreateClient();

            var document = await HtmlDocumentFetcher.FetchAsync(client);

            var mainKpis = LatestStatsParser.Parse(document, fetchedAt);

            Console.WriteLine("Total {0}", mainKpis.Total);
            Console.WriteLine("Recovered {0}", mainKpis.Recovered);
            Console.WriteLine("Deaths {0}", mainKpis.Deaths);

            var activeCases = ActiveCasesParser.Parse(document, fetchedAt);

            Console.WriteLine("TotalActive {0}", activeCases.Total);
            Console.WriteLine("MildActive {0}", activeCases.Mild);
            Console.WriteLine("SeriousActive {0}", activeCases.Serious);

            var closedCases = ClosedCasesParser.Parse(document, fetchedAt);

            Console.WriteLine("TotalClosed {0}", closedCases.Total);
            Console.WriteLine("TotalRecovered {0}", closedCases.Recovered);
            Console.WriteLine("TotalDeaths {0}", closedCases.Deaths);

            foreach (var countryStats in CountryStatsParser.Parse(document, fetchedAt))
            {
                Console.WriteLine(countryStats);
            }

            Debugger.Break();
        }
    }
}