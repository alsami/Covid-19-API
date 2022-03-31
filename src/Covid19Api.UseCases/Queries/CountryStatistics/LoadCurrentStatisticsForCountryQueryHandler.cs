using AutoMapper;
using Covid19Api.Presentation.Response;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.UseCases.Abstractions.Queries.CountryStatistics;
using MediatR;

namespace Covid19Api.UseCases.Queries.CountryStatistics;

public class LoadCurrentStatisticsForCountryQueryHandler : IRequestHandler<LoadCurrentStatisticsForCountryQuery, CountryStatisticDto>
{
    private readonly IMapper mapper;
    private readonly ICountryStatisticsReadRepository countryStatisticsReadRepository;

    public LoadCurrentStatisticsForCountryQueryHandler(IMapper mapper, ICountryStatisticsReadRepository countryStatisticsReadRepository)
    {
        this.mapper = mapper;
        this.countryStatisticsReadRepository = countryStatisticsReadRepository;
    }

    public async Task<CountryStatisticDto> Handle(LoadCurrentStatisticsForCountryQuery request,
        CancellationToken cancellationToken)
    {
        var current = await this.countryStatisticsReadRepository.LoadCurrentAsync(request.Country);

        return this.mapper.Map<CountryStatisticDto>(current);
    }
}