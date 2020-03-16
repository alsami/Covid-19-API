using Autofac;
using Autofac.Extensions.DependencyInjection;
using Covid19Api.AutoMapper.Modules;
using Covid19Api.Repositories;
using Covid19Api.Repositories.Mongo;
using Covid19Api.Services.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Covid19Api.RefreshWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseSerilog(ConfigureLogger)
                .ConfigureAppConfiguration(builder => builder.AddUserSecrets("Covid19ApiRefreshWorker"))
                .ConfigureContainer<ContainerBuilder>(ConfigureContainer)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<DataRefreshWorker>();
                    services.AddHttpClient();
                    services.Configure<DocumentDbContextOptions>(options =>
                        hostContext.Configuration.GetSection(nameof(DocumentDbContextOptions)).Bind(options));
                });

        private static void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<LatestStatsRepository>()
                .AsSelf()
                .InstancePerLifetimeScope();
            containerBuilder.RegisterType<ActiveCasesStatsRepository>()
                .AsSelf()
                .InstancePerLifetimeScope();
            containerBuilder.RegisterType<ClosedCasesRepository>()
                .AsSelf()
                .InstancePerLifetimeScope();
            containerBuilder.RegisterType<CountryStatsRepository>()
                .AsSelf()
                .InstancePerLifetimeScope();
            containerBuilder.RegisterModule(new DocumentDbContextModule(new HostingEnvironment()
            {
                EnvironmentName = "Azure"
            }));
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