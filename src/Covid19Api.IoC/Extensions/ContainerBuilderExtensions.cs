using Autofac;
using Covid19Api.IoC.Modules;
using Covid19Api.Repositories;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.Services.Abstractions.Caching;
using Covid19Api.Services.Abstractions.Parser;
using Covid19Api.Services.Cache;
using Covid19Api.Services.Parser;
using Covid19Api.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Covid19Api.IoC.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterDatabaseDependencies(this ContainerBuilder builder,
            IHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            builder.RegisterType<GlobalStatisticsRepository>()
                .As<IGlobalStatisticsRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CountryStatisticsRepository>()
                .As<ICountryStatisticsRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterModule(new Covid19ApiDbContextModule(hostEnvironment, configuration));

            return builder;
        }

        public static ContainerBuilder RegisterParser(this ContainerBuilder builder)
        {
            builder.RegisterType<CountryStatisticsParser>()
                .As<ICountryStatisticsParser>()
                .SingleInstance();

            builder.RegisterType<GlobalStatisticsParser>()
                .As<IGlobalStatisticsParser>()
                .SingleInstance();

            return builder;
        }

        public static ContainerBuilder RegisterHtmlDocumentCache(this ContainerBuilder builder)
        {
            builder.RegisterType<HtmlDocumentCache>()
                .As<IHtmlDocumentCache>()
                .SingleInstance();

            return builder;
        }

        public static ContainerBuilder RegisterWorker(this ContainerBuilder builder)
        {
            builder.RegisterType<DataRefreshWorker>()
                .As<IHostedService>()
                .InstancePerDependency();

            return builder;
        }
    }
}