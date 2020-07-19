using Autofac;
using Autofac.Extensions.DependencyInjection;
using Covid19Api.AutoMapper.Modules;
using Covid19Api.Repositories;
using Covid19Api.Services.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Covid19Api.RefreshWorker
{
    public static class Program
    {
        private const string UserSecretsId = "Covid19ApiRefreshWorker";
        private const string Environment = "Azure";
        
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseEnvironment(Environment)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseSerilog(ConfigureLogger)
                .ConfigureAppConfiguration(builder => builder.AddUserSecrets(UserSecretsId))
                .ConfigureContainer<ContainerBuilder>(ConfigureContainer)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<DataRefreshWorker>();
                    services.AddHttpClient();
                });

        private static void ConfigureContainer(HostBuilderContext context, ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<GlobalStatsRepository>()
                .AsSelf()
                .InstancePerLifetimeScope();

            containerBuilder.RegisterType<CountryStatsRepository>()
                .AsSelf()
                .InstancePerLifetimeScope();

            containerBuilder.RegisterModule(new DocumentDbContextModule(context.HostingEnvironment, context.Configuration));
        }

        private static void ConfigureLogger(HostBuilderContext context, LoggerConfiguration configuration)
        {
            configuration
                .MinimumLevel.Is(LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                    theme: AnsiConsoleTheme.Literate);
        }
    }
}