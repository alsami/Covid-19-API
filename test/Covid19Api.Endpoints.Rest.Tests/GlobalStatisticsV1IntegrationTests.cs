using Covid19Api.Endpoints.Rest.Tests.Fixtures;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Covid19Api.Endpoints.Rest.Tests;

public class GlobalStatisticsV1IntegrationTests : IClassFixture<WebApplicationFactoryFixture>
{
    private const string ControllerBasePath = "api/v1/global";

    private readonly HttpClient client;
    private readonly ITestOutputHelper testOutputHelper;
    private HttpResponseMessage response = null!;

    public GlobalStatisticsV1IntegrationTests(WebApplicationFactoryFixture webApplicationFactoryFixture, ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
        this.client = webApplicationFactoryFixture.CreateClient();
    }

    [Fact(Skip = "Requires data setup")]
    public async Task GlobalStatisticsAsync_ValidRequest_Succeeds()
    {
        // GivenADefaultEnvironment()
        await this.WhenRequestingCurrentGlobalStatisticsAsync();
        await this.ThenTheResponseShouldBeSuccessfulAsync();
    }

    private async Task WhenRequestingCurrentGlobalStatisticsAsync()
    {
        this.response = await this.client.GetAsync(ControllerBasePath);
    }

    private async Task ThenTheResponseShouldBeSuccessfulAsync()
    {
        this.response.Should().NotBeNull();
        this.testOutputHelper.WriteLine(this.response.StatusCode.ToString());
        this.testOutputHelper.WriteLine(await this.response.Content.ReadAsStringAsync());
        this.response.IsSuccessStatusCode.Should().BeTrue();
    }
}