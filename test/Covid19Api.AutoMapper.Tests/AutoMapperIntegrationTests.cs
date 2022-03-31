using Autofac;
using AutoMapper;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Xunit;

namespace Covid19Api.AutoMapper.Tests;

public class AutoMapperIntegrationTests
{
    [Fact]
    public void AutoMapper_ValidateProfiles()
    {
        var container = new ContainerBuilder()
            .RegisterAutoMapper(typeof(CountryStatisticsProfile).Assembly)
            .Build();

        var mapperConfiguration = container.Resolve<IConfigurationProvider>();
        mapperConfiguration.AssertConfigurationIsValid();
    }
}