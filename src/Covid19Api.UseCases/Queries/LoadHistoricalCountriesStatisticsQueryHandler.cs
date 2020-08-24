using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Covid19Api.Presentation.Response;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.UseCases.Abstractions.Queries;
using MediatR;

namespace Covid19Api.UseCases.Queries
{
    public class LoadHistoricalCountriesStatisticsQueryHandler : IRequestHandler<LoadHistoricalCountriesStatisticsQuery,
        IEnumerable<CountryStatisticsDto>>
    {
        private readonly IMapper mapper;
        private readonly ICountryStatisticsRepository countryStatisticsRepository;

        public LoadHistoricalCountriesStatisticsQueryHandler(IMapper mapper,
            ICountryStatisticsRepository countryStatisticsRepository)
        {
            this.mapper = mapper;
            this.countryStatisticsRepository = countryStatisticsRepository;
        }

        public async Task<IEnumerable<CountryStatisticsDto>> Handle(LoadHistoricalCountriesStatisticsQuery request,
            CancellationToken cancellationToken)
        {
            var countryHistories = await this.countryStatisticsRepository.HistoricalAsync(request.MinFetchedAt);

            return this.mapper.Map<IEnumerable<CountryStatisticsDto>>(countryHistories);
        }
    }
}