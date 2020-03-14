using System;
using System.Security.Authentication;
using Autofac;
using Covid19Api.Extensions;
using Covid19Api.Repositories;
using Covid19Api.Repositories.Mongo;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Covid19Api.Modules
{
    public class DocumentDbContextModule : Module
    {
        private readonly IHostEnvironment hostEnvironment;

        public DocumentDbContextModule(IHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Covid19DbContext>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.Register<Func<IMongoDatabase>>(componentContext =>
                {
                    return () =>
                    {
                        var options = componentContext.Resolve<IOptions<DocumentDbContextOptions>>();

                        var settings = MongoClientSettings.FromUrl(new MongoUrl(options.Value.ConnectionString));

                        if (this.hostEnvironment.IsAzure())
                        {
                            settings.SslSettings = new SslSettings
                            {
                                EnabledSslProtocols = SslProtocols.Tls12
                            };
                        }

                        var client = new MongoClient(settings);

                        return client.GetDatabase(options.Value.DatabaseName);
                    };
                })
                .InstancePerLifetimeScope();
        }
    }
}