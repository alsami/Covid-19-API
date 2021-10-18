using Autofac;
using Covid19Api.IoC.Modules;
using Covid19Api.Repositories;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.Services.Abstractions.Calculators;
using Covid19Api.Services.Abstractions.Compression;
using Covid19Api.Services.Abstractions.Loader;
using Covid19Api.Services.Calculators;
using Covid19Api.Services.Compression;
using Covid19Api.Services.Decorator;
using Covid19Api.Services.Loader;
using Covid19Api.Worker;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Covid19Api.IoC.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterRepositories(this ContainerBuilder builder,
            IHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            builder.RegisterType<GlobalStatisticsReadRepository>()
                .As<IGlobalStatisticsReadRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GlobalStatisticsWriteRepository>()
                .As<IGlobalStatisticsWriteRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GlobalStatisticsAggregatesReadRepository>()
                .As<IGlobalStatisticsAggregatesReadRepository>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<GlobalStatisticsAggregatesWriteRepository>()
                .As<IGlobalStatisticsAggregatesWriteRepository>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<CountryStatisticsReadRepository>()
                .As<ICountryStatisticsReadRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CountryStatisticsWriteRepository>()
                .As<ICountryStatisticsWriteRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CountryStatisticsAggregatesReadRepository>()
                .As<ICountryStatisticsAggregatesReadRepository>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<CountryStatisticsAggregatesWriteRepository>()
                .As<ICountryStatisticsAggregatesWriteRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RequestLogWriteRepository>()
                .As<IRequestLogWriteRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<VaccinationStatisticWriteRepository>()
                .As<IVaccinationStatisticWriteRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<VaccinationStatisticReadRepository>()
                .As<IVaccinationStatisticReadRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterModule(new Covid19ApiDbContextModule(hostEnvironment, configuration));

            return builder;
        }

        public static ContainerBuilder RegisterServices(this ContainerBuilder builder)
        {
            builder.RegisterType<CountryVaryStatisticsCalculator>()
                .As<ICountryVaryStatisticsCalculator>()
                .SingleInstance();
            
            builder.RegisterType<BrotliCompressionService>()
                .As<ICompressionService>()
                .SingleInstance();

            builder.RegisterType<CountryStatisticsLoader>()
                .As<ICountryStatisticsLoader>()
                .SingleInstance();

            builder.RegisterType<GlobalStatisticsLoader>()
                .As<IGlobalStatisticsLoader>()
                .SingleInstance();

            builder.Register(_ =>
                    new MemoryDistributedCache(
                        Options.Create(new MemoryDistributedCacheOptions())))
                .As<IDistributedCache>()
                .SingleInstance();

            builder.RegisterType<HtmlDocumentLoader>()
                .As<IHtmlDocumentLoader>()
                .SingleInstance();

            builder.RegisterType<CountryMetaDataLoader>()
                .As<ICountryMetaDataLoader>()
                .SingleInstance();

            builder.RegisterType<VaccinationStatisticsLoader>()
                .As<IVaccinationStatisticsLoader>()
                .SingleInstance();

            builder.RegisterDecorator<CountryMetaDataLoaderDecorator, ICountryMetaDataLoader>();
            builder.RegisterDecorator<HtmlDocumentLoaderDecorator, IHtmlDocumentLoader>();

            return builder;
        }

        public static ContainerBuilder RegisterWorker(this ContainerBuilder builder, IHostEnvironment hostEnvironment)
        {
            if (hostEnvironment.IsContinuousIntegration())
            {
                return builder;
            }

            builder.RegisterType<GlobalStatisticsRefreshWorker>()
                .As<IHostedService>()
                .InstancePerDependency();
            
            builder.RegisterType<CountryStatisticsRefreshWorker>()
                .As<IHostedService>()
                .InstancePerDependency();

            builder.RegisterType<VaccinationStatisticsRefreshWorker>()
                .As<IHostedService>()
                .InstancePerDependency();
            
            builder.RegisterType<GlobalStatisticsAggregationWorker>()
                .As<IHostedService>()
                .InstancePerDependency();

            builder.RegisterType<CountryStatisticsAggregateWorker>()
                .As<IHostedService>()
                .InstancePerDependency();

            return builder;
        }
    }
}