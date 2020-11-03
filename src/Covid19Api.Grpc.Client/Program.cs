using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Covid19Api.Grpc.CountryStatistics;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;

namespace Covid19Api.Grpc.Client
{
#pragma warning disable S1075
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5000", new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.Insecure,
                
            });
            var client = new CountryStatisticsService.CountryStatisticsServiceClient(channel)
            {
                
            };
            var sw = Stopwatch.StartNew();
            var res = await client.LoadLatestCountryStatisticsAsync(new Empty());
            sw.Stop();
            Console.WriteLine("GRPC:");
            Console.WriteLine(sw.ElapsedMilliseconds / 1000m);
            Console.WriteLine(res != null);
            var http = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:5001/api/v1")
            };
            sw = Stopwatch.StartNew();
            var resHttp = await http.GetAsync(http.BaseAddress + "/countries");
            var deserialized = JsonSerializer.Deserialize<CountryStatisticsGrpcMessage[]>(await resHttp.Content.ReadAsStringAsync(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() },
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            sw.Stop();
            Console.WriteLine("HTTP:");
            Console.WriteLine(sw.ElapsedMilliseconds / 1000m);
            Console.WriteLine(deserialized != null);
        }
    }
#pragma  warning restore
}