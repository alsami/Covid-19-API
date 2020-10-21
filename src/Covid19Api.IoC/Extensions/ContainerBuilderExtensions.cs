using Autofac;
using Covid19Api.IoC.Modules;
using Covid19Api.Repositories;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.Services.Abstractions.Compression;
using Covid19Api.Services.Abstractions.Loader;
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
            builder.RegisterType<GlobalStatisticsRepository>()
                .As<IGlobalStatisticsRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GlobalStatisticsAggregatesRepository>()
                .As<IGlobalStatisticsAggregatesRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CountryStatisticsRepository>()
                .As<ICountryStatisticsRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CountryStatisticsAggregatesRepository>()
                .As<ICountryStatisticsAggregatesRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterModule(new Covid19ApiDbContextModule(hostEnvironment, configuration));

            return builder;
        }

        public static ContainerBuilder RegisterServices(this ContainerBuilder builder)
        {
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

            builder.RegisterDecorator<CountryMetaDataLoaderDecorator, ICountryMetaDataLoader>();
            builder.RegisterDecorator<HtmlDocumentLoaderDecorator, IHtmlDocumentLoader>();

            return builder;
        }

        public static ContainerBuilder RegisterWorker(this ContainerBuilder builder)
        {
            builder.RegisterType<DataRefreshWorker>()
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