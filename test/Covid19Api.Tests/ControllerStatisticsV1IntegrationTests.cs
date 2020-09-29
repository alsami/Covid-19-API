using System.Net.Http;
using System.Threading.Tasks;
using Covid19Api.Tests.Fixtures;
using FluentAssertions;
using Xunit;

namespace Covid19Api.Tests
{
    public class GlobalStatisticsV1IntegrationTests : IClassFixture<WebApplicationFactoryFixture>
    {
        private const string ControllerBasePath = "api/v1/global";

        private readonly HttpClient client;
        private HttpResponseMessage response = null!;

        public GlobalStatisticsV1IntegrationTests(WebApplicationFactoryFixture webApplicationFactoryFixture)
        {
            this.client = webApplicationFactoryFixture.CreateClient();
        }

        [Fact]
        public async Task GlobalStatisticsAsync_ValidRequest_Succeeds()
        {
            // GivenADefaultEnvironment()
            await this.WhenRequestingCurrentGlobalStatisticsAsync();
            this.ThenTheResponseShouldBeSuccessful();
        }

        private async Task WhenRequestingCurrentGlobalStatisticsAsync()
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