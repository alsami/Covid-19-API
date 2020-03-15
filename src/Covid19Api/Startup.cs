using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Covid19Api.AutoMapper.Modules;
using Covid19Api.ExceptionFilter;
using Covid19Api.Repositories;
using Covid19Api.Repositories.Mongo;
using Covid19Api.Services;
using Covid19Api.Services.Worker;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Covid19Api
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment webHostEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            this.configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options => { options.Filters.Add<UnhandledExceptionFilter>(); });

            services.AddHttpClient();

            services.AddCors(options => options.AddPolicy("DefaultCorsPolicy",
                builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

            services.AddHostedService<DataRefreshWorker>();

            services.Configure<DocumentDbContextOptions>(options =>
                this.configuration.GetSection(nameof(DocumentDbContextOptions)).Bind(options));
        }

        // ReSharper disable once UnusedMember.Global
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddAutoMapper(typeof(Startup).Assembly);

            containerBuilder.RegisterType<LatestStatsRepository>()
                .AsSelf()
                .InstancePerLifetimeScope();
            containerBuilder.RegisterType<ActiveCasesStatsRepository>()
                .AsSelf()
                .InstancePerLifetimeScope();
            containerBuilder.RegisterType<ClosedCasesRepository>()
                .AsSelf()
                .InstancePerLifetimeScope();
            containerBuilder.RegisterType<CountryStatsRepository>()
                .AsSelf()
                .InstancePerLifetimeScope();
            containerBuilder.RegisterModule(new DocumentDbContextModule(this.webHostEnvironment));
        }

        // ReSharper disable once UnusedMember.Global
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("DefaultCorsPolicy");

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}