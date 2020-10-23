using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Covid19Api.Presentation.Response;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.UseCases.Abstractions.Queries.CountryStatisticsAggregates;
using MediatR;

namespace Covid19Api.UseCases.Queries.CountryStatisticsAggregates
{
    public class LoadCountryStatisticsAggregatesForCountryInYearQueryHandler : IRequestHandler<
        LoadCountryStatisticsAggregatesForCountryInYearQuery, IEnumerable<CountryStatisticsAggregateDto>>
    {
        private readonly IMapper mapper;
        private readonly ICountryStatisticsAggregatesRepository countryStatisticsAggregatesRepository;

        public LoadCountryStatisticsAggregatesForCountryInYearQueryHandler(IMapper mapper,
            ICountryStatisticsAggregatesRepository countryStatisticsAggregatesRepository)
        {
            this.mapper = mapper;
            this.countryStatisticsAggregatesRepository = countryStatisticsAggregatesRepository;
        }

        public async Task<IEnumerable<CountryStatisticsAggregateDto>> Handle(
            LoadCountryStatisticsAggregatesForCountryInYearQuery request, CancellationToken cancellationToken)
        {
            var aggregates =
                await this.countryStatisticsAggregatesRepository.FindForCountryInYearAsync(request.Country,
                    request.Year);

            return aggregates.Any()
                ? this.mapper.Map<IEnumerable<CountryStatisticsAggregateDto>>(aggregates)
                : Array.Empty<CountryStatisticsAggregateDto>();
        }
    }
}