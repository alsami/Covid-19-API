using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Covid19Api.Grpc.CountryStatistics;
using Grpc.Net.Client;

namespace Covid19Api.Grpc.Client
{
#pragma warning disable S1075
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new CountryStatisticsService.CountryStatisticsServiceClient(channel);
            var sw = Stopwatch.StartNew();
            var res = await client.LoadHistoricalCountryStatisticForCountryAsync(new CountryStatisticsForCountryGrpcMessage()
            {
                Country = "Germany"
            });
            sw.Stop();
            Console.WriteLine("GRPC:");
            Console.WriteLine(sw.ElapsedMilliseconds / 1000m);
            Console.WriteLine(res.ToString());
            var http = new HttpClient()
            {
                BaseAddress = new Uri("https://localhost:5001/api/v1")
            };
            sw = Stopwatch.StartNew();
            var resHttp = await http.GetAsync(http.BaseAddress + "/countries/germany/history");
            sw.Stop();
            Console.WriteLine("HTTP:");
            Console.WriteLine(sw.ElapsedMilliseconds / 1000m);
            Console.WriteLine(await resHttp.Content.ReadAsStringAsync());
        }
    }
#pragma  warning restore
}