using Covid19Api.Presentation.Response;
using Covid19Api.UseCases.Abstractions.Queries.Countries;
using Covid19Api.UseCases.Abstractions.Queries.CountryStatistics;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Covid19Api.Endpoints.Rest.V1;

[ApiController]
[Route("api/v1/countries")]
public class CountryStatisticsController : ControllerBase
{
    private readonly IMediator mediator;

    public CountryStatisticsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public Task<IEnumerable<CountryStatisticDto>> LoadLatestAsync() =>
        this.mediator.Send(new LoadCurrentStatisticsForCountyQuery());

    [HttpGet("history")]
    public Task<IEnumerable<CountryStatisticDto>> LoadHistoryAsync()
        => this.mediator.Send(new LoadHistoricalCountriesStatisticsQuery(DateTime.UtcNow.Date.AddDays(-9)));

    [HttpGet("history/vary")]
    public Task<IEnumerable<CountryVaryStatisticContainerDto>> LoadVaryHistoryAsync()
        => this.mediator.Send(new CalculateVaryForCountriesStatisticsQuery(DateTime.UtcNow.Date.AddDays(-9)));

    [HttpGet("{country}")]
    public Task<CountryStatisticDto> LoadLatestForCountryAsync(string country) =>
        this.mediator.Send(new LoadCurrentStatisticsForCountryQuery(country));

    [HttpGet("{country}/history")]
    public Task<IEnumerable<CountryStatisticDto>> LoadHistoryForCountryAsync(string country) =>
        this.mediator.Send(
            new LoadHistoricalCountryStatisticsForCountryQuery(country, DateTime.UtcNow.Date.AddDays(-9)));

    [HttpGet("{country}/history/vary")]
    public Task<IEnumerable<CountryVaryStatisticContainerDto>> LoadVaryHistoryForCountryAsync(string country) =>
        this.mediator.Send(new CalculateVaryForCountryStatisticsQuery(country, DateTime.UtcNow.Date.AddDays(-9)));

    [HttpGet("{countryCode}/flag")]
    public async Task<IActionResult> LoadFlagAsync(string countryCode)
    {
        var (image, mimeType) = await this.mediator.Send(new LoadCountryFlagQuery(countryCode));

        return new FileContentResult(image, mimeType)
        {
            EnableRangeProcessing = true,
            FileDownloadName = $"{countryCode.ToLowerInvariant()}_flag",
            LastModified = DateTimeOffset.MinValue,
        };
    }
}