using Covid19Api.Repositories.Abstractions;
using Covid19Api.Services.Abstractions.Loader;
using Covid19Api.UseCases.Abstractions.Commands;
using Covid19Api.UseCases.Filter;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Covid19Api.UseCases.Commands;

public class RefreshCountriesStatisticsCommandHandler : IRequestHandler<RefreshCountriesStatisticsCommand>
{
    private readonly ILogger<RefreshCountriesStatisticsCommandHandler> logger;
    private readonly ICountryStatisticsWriteRepository countryStatisticsWriteRepository;
    private readonly ICountryStatisticsLoader countryStatisticsLoader;

    public RefreshCountriesStatisticsCommandHandler(ILogger<RefreshCountriesStatisticsCommandHandler> logger,
        ICountryStatisticsWriteRepository countryStatisticsWriteRepository,
        ICountryStatisticsLoader countryStatisticsLoader)
    {
        this.logger = logger;
        this.countryStatisticsWriteRepository = countryStatisticsWriteRepository;
        this.countryStatisticsLoader = countryStatisticsLoader;
    }

    public async Task<Unit> Handle(RefreshCountriesStatisticsCommand request, CancellationToken cancellationToken)
    {
        var countriesStatistics =
            (await this.countryStatisticsLoader.ParseAsync(request.FetchedAt, CountryStatsFilter.ValidOnly.Value))
            .ToList();

        var countriesWithoutCountryCode = countriesStatistics
            .Where(statistics => string.IsNullOrWhiteSpace(statistics!.CountryCode))
            .ToList();

        if (countriesWithoutCountryCode.Any())
        {
            this.logger.LogWarning("There are countries without a country-code! {countries}",
                string.Join(", ", countriesWithoutCountryCode.Select(statistics => statistics!.Country)));
        }

        await this.countryStatisticsWriteRepository.StoreManyAsync(countriesStatistics!);

        return Unit.Value;
    }
}