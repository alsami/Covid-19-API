using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Covid19Api.Presentation.Response;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.UseCases.Abstractions.Queries.CountryStatisticsAggregates;
using MediatR;

namespace Covid19Api.UseCases.Queries.CountryStatisticsAggregates
{
    public class
        LoadCountryStatisticsAggregateQueryHandler : IRequestHandler<LoadCountryStatisticsAggregate,
            CountryStatisticAggregateDto?>
    {
        private readonly IMapper mapper;
        private readonly ICountryStatisticsAggregatesReadRepository countryStatisticsAggregatesReadRepository;


        public LoadCountryStatisticsAggregateQueryHandler(IMapper mapper,
            ICountryStatisticsAggregatesReadRepository countryStatisticsAggregatesReadRepository)
        {
            this.mapper = mapper;
            this.countryStatisticsAggregatesReadRepository = countryStatisticsAggregatesReadRepository;
        }

        public async Task<CountryStatisticAggregateDto?> Handle(LoadCountryStatisticsAggregate request,
            CancellationToken cancellationToken)
        {
            var aggregate =
                await this.countryStatisticsAggregatesReadRepository.FindAsync(request.Country, request.Month,
                    request.Year);

            return aggregate is null
                ? null
                : this.mapper.Map<CountryStatisticAggregateDto>(aggregate);
        }
    }
}