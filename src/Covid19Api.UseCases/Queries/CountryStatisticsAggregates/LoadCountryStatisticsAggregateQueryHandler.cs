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
            CountryStatisticsAggregateDto?>
    {
        private readonly IMapper mapper;
        private readonly ICountryStatisticsAggregatesRepository countryStatisticsAggregatesRepository;


        public LoadCountryStatisticsAggregateQueryHandler(IMapper mapper,
            ICountryStatisticsAggregatesRepository countryStatisticsAggregatesRepository)
        {
            this.mapper = mapper;
            this.countryStatisticsAggregatesRepository = countryStatisticsAggregatesRepository;
        }

        public async Task<CountryStatisticsAggregateDto?> Handle(LoadCountryStatisticsAggregate request,
            CancellationToken cancellationToken)
        {
            var aggregate =
                await this.countryStatisticsAggregatesRepository.FindAsync(request.Country, request.Month,
                    request.Year);

            return aggregate is null
                ? null
                : this.mapper.Map<CountryStatisticsAggregateDto>(aggregate);
        }
    }
}