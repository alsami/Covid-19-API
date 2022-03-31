using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;
using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Covid19Api.ActionFilter;
using Covid19Api.AutoMapper;
using Covid19Api.ExceptionFilter;
using Covid19Api.IoC.Extensions;
using Covid19Api.Middleware;
using Covid19Api.Services.Configuration;
using Covid19Api.UseCases.Behaviors;
using Covid19Api.UseCases.Queries.GlobalStatistics;
using MediatR.Extensions.Autofac.DependencyInjection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;

namespace Covid19Api;

public class Startup
{
    private readonly IConfiguration configuration;
    private readonly IWebHostEnvironment hostEnvironment;
    private const string ApiName = "Covid19-API";
    private const string ApiVersion = "1.0.0";

    private const string CorsPolicyName = "DefaultCorsPolicy";
    private const string EnableResponseCompressionKey = "EnableResponseCompression";

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
            options.Filters.Add<RequestStoreActionFilter>();
        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        });

        services.AddSwaggerGen(options => options.SwaggerDoc(ApiVersion, new OpenApiInfo
        {
            Title = $"{ApiName} - {ApiVersion}",
            Version = ApiVersion
        }));

        services.AddOptions<CountryLayerConfiguration>()
            .BindConfiguration(nameof(CountryLayerConfiguration));

        services.AddHttpClient();

        services.AddCors(options => options.AddPolicy(CorsPolicyName, builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()));

        if (this.configuration.GetSection(EnableResponseCompressionKey).Get<bool>())
        {
            services.AddResponseCompression(options => options.MimeTypes = this.compressionMimeTypes);

            services.Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);   
        }

        services.AddScoped<RedirectDefaultRequestMiddleware>();
    }

    // ReSharper disable once UnusedMember.Global
    public void ConfigureContainer(ContainerBuilder containerBuilder)
    {
        var behaviors = new List<Type>();

        if (!this.configuration.GetSection("DisableCachingBehavior").Get<bool>())
        {
            behaviors.Add(typeof(CachingBehavior<,>));
        }
            
        containerBuilder
            .RegisterAutoMapper(typeof(CountryStatisticsProfile).Assembly)
            .RegisterMediatR(typeof(LoadLatestGlobalStatisticsQueryHandler).Assembly, behaviors.ToArray())
            .RegisterServices()
            .RegisterRepositories(this.hostEnvironment, this.configuration);

        if (this.configuration.GetSection("DisableWorker").Get<bool>())
        {
            return;
        }

        containerBuilder.RegisterWorker(this.hostEnvironment);
    }

    // ReSharper disable once UnusedMember.Global
    public void Configure(IApplicationBuilder app)
    {
        app.UseMiddleware<RedirectDefaultRequestMiddleware>();

        app
            .UseMiddleware<RedirectDefaultRequestMiddleware>()
            .UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

        if (this.configuration.GetSection(EnableResponseCompressionKey).Get<bool>())
        {
            app.UseResponseCompression();    
        } 

        app
            .UseSwagger()
            .UseSwaggerUI(options => options.SwaggerEndpoint($"/swagger/{ApiVersion}/swagger.json", ApiName))
            .UseRouting()
            .UseCors(CorsPolicyName)
            .UseEndpoints(endpoints => endpoints.MapControllers());
    }
}