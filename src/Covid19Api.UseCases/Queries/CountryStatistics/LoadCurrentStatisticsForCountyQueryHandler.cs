using AutoMapper;
using Covid19Api.Presentation.Response;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.UseCases.Abstractions.Queries.CountryStatistics;
using MediatR;

namespace Covid19Api.UseCases.Queries.CountryStatistics;

public class LoadCurrentStatisticsForCountyQueryHandler : IRequestHandler<LoadCurrentStatisticsForCountyQuery, IEnumerable<CountryStatisticDto>>
{
    private readonly IMapper mapper;
    private readonly ICountryStatisticsReadRepository countryStatisticsReadRepository;


    public LoadCurrentStatisticsForCountyQueryHandler(IMapper mapper, ICountryStatisticsReadRepository countryStatisticsReadRepository)
    {
        this.mapper = mapper;
        this.countryStatisticsReadRepository = countryStatisticsReadRepository;
    }

    public async Task<IEnumerable<CountryStatisticDto>> Handle(LoadCurrentStatisticsForCountyQuery request,
        CancellationToken cancellationToken)
    {
        var current = await this.countryStatisticsReadRepository.LoadCurrentAsync();

        return this.mapper.Map<IEnumerable<CountryStatisticDto>>(current).OrderByDescending(country => country!.TotalCases);
    }
}