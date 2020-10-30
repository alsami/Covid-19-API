using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;
using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Covid19Api.AutoMapper;
using Covid19Api.Endpoints.Grpc;
using Covid19Api.ExceptionFilter;
using Covid19Api.GrpcInterceptors;
using Covid19Api.IoC.Extensions;
using Covid19Api.Middleware;
using Covid19Api.UseCases.Behaviors;
using Covid19Api.UseCases.Queries.GlobalStatistics;
using MediatR.Extensions.Autofac.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using GrpcGzipCompressionProvider = Grpc.Net.Compression.GzipCompressionProvider;

namespace Covid19Api
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment hostEnvironment;
        private const string ApiName = "Covid19-API";
        private const string ApiVersion = "1.0.0";

        private const string CorsPolicyName = "DefaultCorsPolicy";

        private readonly string[] compressionMimeTypes =
        {
            "application/json"
        };

        public Startup(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            this.configuration = configuration;
            this.hostEnvironment = hostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<UnhandledExceptionFilter>();
                options.Filters.Add<AzureCosmosDbThrottleExceptionFilter>();
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            });

            services.AddGrpc(opt =>
            {
                opt.EnableDetailedErrors = true;
                opt.CompressionProviders.Insert(0, new GrpcGzipCompressionProvider(CompressionLevel.Fastest));
                opt.ResponseCompressionLevel = CompressionLevel.Fastest;
                opt.Interceptors.Add<NoopInterceptor>();
            });

            services.AddSwaggerGen(options => options.SwaggerDoc(ApiVersion, new OpenApiInfo
            {
                Title = $"{ApiName} - {ApiVersion}",
                Version = ApiVersion
            }));

            services.AddHttpClient();

            services.AddCors(options => options.AddPolicy(CorsPolicyName, builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));

            services.AddResponseCompression(options => options.MimeTypes = this.compressionMimeTypes);

            services.Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

            services.AddScoped<RedirectDefaultRequestMiddleware>();
        }

        // ReSharper disable once UnusedMember.Global
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterAutoMapper(typeof(CountryStatisticsProfile).Assembly)
                .RegisterMediatR(typeof(LoadLatestGlobalStatisticsQueryHandler).Assembly, typeof(CachingBehavior<,>))
                .RegisterWorker(this.hostEnvironment)
                .RegisterServices()
                .RegisterRepositories(this.hostEnvironment, this.configuration);
        }

        // ReSharper disable once UnusedMember.Global
        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<RedirectDefaultRequestMiddleware>();

            app
                .UseResponseCompression()
                .UseSwagger()
                .UseSwaggerUI(options => options.SwaggerEndpoint($"/swagger/{ApiVersion}/swagger.json", ApiName))
                .UseRouting()
                .UseCors(CorsPolicyName)
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapGrpcService<GlobalStatisticsServiceGrpc>();
                    endpoints.MapGrpcService<CountryStatisticsServiceGrpc>();
                    endpoints.MapControllers();
                });
        }
    }
}