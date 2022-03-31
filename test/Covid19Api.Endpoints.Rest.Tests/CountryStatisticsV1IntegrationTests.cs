using Covid19Api.Endpoints.Rest.Tests.Fixtures;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Covid19Api.Endpoints.Rest.Tests;

public class CountryStatisticsV1IntegrationTests : IClassFixture<WebApplicationFactoryFixture>
{
    private const string ControllerBasePath = "api/v1/countries";

    private readonly HttpClient client;
    private readonly ITestOutputHelper testOutputHelper;
    private HttpResponseMessage response = null!;

    public CountryStatisticsV1IntegrationTests(WebApplicationFactoryFixture webApplicationFactoryFixture, ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
        this.client = webApplicationFactoryFixture.CreateClient();
    }

    [Fact]
    public async Task CountryStatisticsAsync_ValidRequest_Succeeds()
    {
        // GivenADefaultEnvironment()
        await this.WhenRequestingCurrentCountryStatisticsAsync();
        await this.ThenTheResponseShouldBeSuccessfulAsync();
    }

    private async Task WhenRequestingCurrentCountryStatisticsAsync()
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