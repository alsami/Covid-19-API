using AutoMapper;
using Covid19Api.Presentation.Response;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.UseCases.Abstractions.Queries.CountryStatisticsAggregates;
using MediatR;

namespace Covid19Api.UseCases.Queries.CountryStatisticsAggregates;

public class LoadCountryStatisticsAggregatesForCountryInYearQueryHandler : IRequestHandler<
    LoadCountryStatisticsAggregatesForCountryInYearQuery, IEnumerable<CountryStatisticAggregateDto>>
{
    private readonly IMapper mapper;
    private readonly ICountryStatisticsAggregatesReadRepository countryStatisticsAggregatesReadRepository;

    public LoadCountryStatisticsAggregatesForCountryInYearQueryHandler(IMapper mapper,
        ICountryStatisticsAggregatesReadRepository countryStatisticsAggregatesReadRepository)
    {
        this.mapper = mapper;
        this.countryStatisticsAggregatesReadRepository = countryStatisticsAggregatesReadRepository;
    }

    public async Task<IEnumerable<CountryStatisticAggregateDto>> Handle(
        LoadCountryStatisticsAggregatesForCountryInYearQuery request, CancellationToken cancellationToken)
    {
        var aggregates =
            await this.countryStatisticsAggregatesReadRepository.FindForCountryInYearAsync(request.Country,
                request.Year);

        return aggregates.Any()
            ? this.mapper.Map<IEnumerable<CountryStatisticAggregateDto>>(aggregates)
            : Array.Empty<CountryStatisticAggregateDto>();
    }
}