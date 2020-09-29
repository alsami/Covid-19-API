using System.Net.Http;
using System.Threading.Tasks;
using Covid19Api.Tests.Fixtures;
using FluentAssertions;
using Xunit;

namespace Covid19Api.Tests
{
    public class CountryStatisticsV1IntegrationTests : IClassFixture<WebApplicationFactoryFixture>
    {
        private const string ControllerBasePath = "api/v1/countries";

        private readonly HttpClient client;
        private HttpResponseMessage response = null!;

        public CountryStatisticsV1IntegrationTests(WebApplicationFactoryFixture webApplicationFactoryFixture)
        {
            this.client = webApplicationFactoryFixture.CreateClient();
        }

        [Fact]
        public async Task CountryStatisticsAsync_ValidRequest_Succeeds()
        {
            // GivenADefaultEnvironment()
            await this.WhenRequestingCurrentCountryStatisticsAsync();
            this.ThenTheResponseShouldBeSuccessful();
        }

        private async Task WhenRequestingCurrentCountryStatisticsAsync()
        {
            this.response = await this.client.GetAsync(ControllerBasePath);
        }

        private void ThenTheResponseShouldBeSuccessful()
        {
            this.response.Should().NotBeNull();
            this.response.IsSuccessStatusCode.Should().BeTrue();
        }
    }
}