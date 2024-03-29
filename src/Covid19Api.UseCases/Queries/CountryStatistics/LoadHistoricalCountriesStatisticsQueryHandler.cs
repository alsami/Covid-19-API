using AutoMapper;
using Covid19Api.Presentation.Response;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.UseCases.Abstractions.Queries.CountryStatistics;
using MediatR;

namespace Covid19Api.UseCases.Queries.CountryStatistics;

public class LoadHistoricalCountriesStatisticsQueryHandler : IRequestHandler<LoadHistoricalCountriesStatisticsQuery,
    IEnumerable<CountryStatisticDto>>
{
    private readonly IMapper mapper;
    private readonly ICountryStatisticsReadRepository countryStatisticsReadRepository;

    public LoadHistoricalCountriesStatisticsQueryHandler(IMapper mapper,
        ICountryStatisticsReadRepository countryStatisticsReadRepository)
    {
        this.mapper = mapper;
        this.countryStatisticsReadRepository = countryStatisticsReadRepository;
    }

    public async Task<IEnumerable<CountryStatisticDto>> Handle(LoadHistoricalCountriesStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        var countryHistories = await this.countryStatisticsReadRepository.HistoricalAsync(request.MinFetchedAt);

        return this.mapper.Map<IEnumerable<CountryStatisticDto>>(countryHistories);
    }
}