using System.IO.Compression;
using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Covid19Api.ExceptionFilter;
using Covid19Api.IoC.Extensions;
using Covid19Api.UseCases.Queries;
using MediatR.Extensions.Autofac.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Covid19Api
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment webHostEnvironment;

        private const string CorsPolicyName = "DefaultCorsPolicy";

        private readonly string[] compressionMimeTypes = new[]
        {
            "application/json"
        };

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            this.configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<UnhandledExceptionFilter>();
                options.Filters.Add<AzureCosmosDbThrottleExceptionFilter>();
            });

            services.AddHttpClient();

            services.AddCors(options => options.AddPolicy(CorsPolicyName, builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));

            services.AddResponseCompression(options => options.MimeTypes = this.compressionMimeTypes);

            services.Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

            services.AddMemoryCache();
        }

        // ReSharper disable once UnusedMember.Global
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder
                .AddAutoMapper(typeof(Startup).Assembly)
                .AddMediatR(typeof(LoadHtmlDocumentQueryHandler).Assembly)
                .RegisterWorker()
                .RegisterParser()
                .RegisterHtmlDocumentCache()
                .RegisterDatabaseDependencies(this.webHostEnvironment, this.configuration);
        }

        // ReSharper disable once UnusedMember.Global
        public void Configure(IApplicationBuilder app)
        {
            app.UseResponseCompression()
                .UseRouting()
                .UseCors(CorsPolicyName)
                .UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}