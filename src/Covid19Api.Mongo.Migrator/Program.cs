using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Covid19Api.AutoMapper;
using Covid19Api.IoC.Extensions;
using Covid19Api.Mongo.Migrator.Abstractions;
using Covid19Api.Mongo.Migrator.Configuration;
using Covid19Api.UseCases.Commands;
using MediatR.Extensions.Autofac.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Covid19Api.Mongo.Migrator
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            using var host = CreateHost(args);

            await host.StartAsync();

            var migrations = host.Services.GetServices<DatabaseMigration>();

            foreach (var databaseMigration in migrations.OrderBy(migration => migration.Number))
                await databaseMigration.ExecuteUpdateAsync();

            await host.StopAsync();
        }

        private static IHost CreateHost(string[] args)
            => Host.CreateDefaultBuilder(args)
                .UseContentRoot(AppContext.BaseDirectory)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseSerilog(ConfigureLogger)
                .ConfigureServices(ConfigureServices)
                .ConfigureContainer<ContainerBuilder>(ConfigureContainer)
                .Build();

        private static void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services)
        {
            services.AddOptions();

            services.AddHttpClient();

            services.Configure<GlobalAggregatesStartConfiguration>(options =>
                hostBuilderContext.Configuration.GetSection(nameof(GlobalAggregatesStartConfiguration)).Bind(options));

            services.Configure<CountryAggregatesStartConfiguration>(options =>
                hostBuilderContext.Configuration.GetSection(nameof(CountryAggregatesStartConfiguration)).Bind(options));
        }

        private static void ConfigureLogger(HostBuilderContext context, LoggerConfiguration loggerConfiguration)
        {
            loggerConfiguration.ReadFrom.Configuration(context.Configuration);
        }

        private static void ConfigureContainer(HostBuilderContext context, ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .AssignableTo(typeof(DatabaseMigration))
                .As<DatabaseMigration>()
                .InstancePerLifetimeScope();

            builder.RegisterRepositories(context.HostingEnvironment, context.Configuration)
                .RegisterServices()
                .RegisterMediatR(typeof(RefreshGlobalStatisticsCommandHandler).Assembly)
                .RegisterAutoMapper(typeof(CountryStatsProfile).Assembly);
        }
    }
}