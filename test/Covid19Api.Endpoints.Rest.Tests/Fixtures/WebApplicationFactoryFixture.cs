using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Covid19Api.Endpoints.Rest.Tests.Fixtures;

public class WebApplicationFactoryFixture : WebApplicationFactory<Startup>
{
    protected override IHostBuilder CreateHostBuilder()
    {
        return base.CreateHostBuilder()
            .UseEnvironment("CI");
    }
}