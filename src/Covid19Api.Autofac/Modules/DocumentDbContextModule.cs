using System;
using System.Security.Authentication;
using Autofac;
using Covid19Api.AutoMapper.Extensions;
using Covid19Api.Repositories.Mongo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Module = Autofac.Module;

namespace Covid19Api.AutoMapper.Modules
{
    public class DocumentDbContextModule : Module
    {
        private const string DatabaseName = "Covid19Api";
        
        private readonly IHostEnvironment hostEnvironment;
        private readonly IConfiguration configuration;

        public DocumentDbContextModule(IHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            this.hostEnvironment = hostEnvironment;
            this.configuration = configuration;
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
                        var connectionString = this.configuration.GetConnectionString("MongoDb");
                        
                        if (string.IsNullOrWhiteSpace(connectionString)) throw new InvalidOperationException("Database connection-string is invalid!"); 
                        
                        var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));

                        if (this.hostEnvironment.IsAzure())
                        {
                            settings.SslSettings = new SslSettings
                            {
                                EnabledSslProtocols = SslProtocols.Tls12
                            };
                        }

                        var client = new MongoClient(settings);

                        return client.GetDatabase(DatabaseName);
                    };
                })
                .InstancePerLifetimeScope();
        }
    }
}