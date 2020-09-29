using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Covid19Api
{
    public static class Program
    {
        private const string UserSecretsId = "Covid19Api";

        public static async Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();

            await host.RunAsync();
        }

        // ReSharper disable once MemberCanBePrivate.Global
        // public for functional-tests
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseSerilog(ConfigureLogger)
                .ConfigureAppConfiguration(builder => builder.AddUserSecrets(UserSecretsId))
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());

        private static void ConfigureLogger(HostBuilderContext context, LoggerConfiguration loggerConfiguration)
        {
            loggerConfiguration.ReadFrom.Configuration(context.Configuration);
        }
    }
}