using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Covid19Api.IoC.Extensions;
using Covid19Api.Mongo.Scaffolder.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Covid19Api.Mongo.Scaffolder
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            using var host = CreateHost(args);

            await host.StartAsync();

            var databaseDefinitions = host.Services.GetServices<DatabaseUpdateDefinition>();

            foreach (var databaseDefinition in databaseDefinitions.OrderBy(update => update.Version))
                await databaseDefinition.ExecuteUpdateAsync();

            await host.StopAsync();
        }

        private static IHost CreateHost(string[] args)
            => Host.CreateDefaultBuilder(args)
                .UseContentRoot(AppContext.BaseDirectory)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseSerilog(ConfigureLogger)
                .ConfigureContainer<ContainerBuilder>(ConfigureContainer)
                .Build();

        private static void ConfigureLogger(HostBuilderContext context, LoggerConfiguration loggerConfiguration)
        {
            loggerConfiguration.ReadFrom.Configuration(context.Configuration);
        }

        private static void ConfigureContainer(HostBuilderContext context, ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .AssignableTo(typeof(DatabaseUpdateDefinition))
                .As<DatabaseUpdateDefinition>()
                .InstancePerDependency();

            builder.RegisterRepositories(context.HostingEnvironment, context.Configuration);
        }
    }
}