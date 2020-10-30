using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Covid19Api.Presentation.Response;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.UseCases.Abstractions.Queries.GlobalStatisticsAggregates;
using MediatR;

namespace Covid19Api.UseCases.Queries.GlobalStatisticsAggregates
{
    public class LoadGlobalStatisticsAggregatesForYearQueryHandler : IRequestHandler<
        LoadGlobalStatisticsAggregatesForYearQuery, IEnumerable<GlobalStatisticAggregateDto>>
    {
        private readonly IMapper mapper;
        private readonly IGlobalStatisticsAggregatesRepository globalStatisticsAggregatesRepository;

        public LoadGlobalStatisticsAggregatesForYearQueryHandler(IMapper mapper,
            IGlobalStatisticsAggregatesRepository globalStatisticsAggregatesRepository)
        {
            this.mapper = mapper;
            this.globalStatisticsAggregatesRepository = globalStatisticsAggregatesRepository;
        }

        public async Task<IEnumerable<GlobalStatisticAggregateDto>> Handle(
            LoadGlobalStatisticsAggregatesForYearQuery request, CancellationToken cancellationToken)
        {
            var aggregates = await this.globalStatisticsAggregatesRepository.FindInYearAsync(request.Year);

            return aggregates.Any()
                ? this.mapper.Map<IEnumerable<GlobalStatisticAggregateDto>>(aggregates)
                : Array.Empty<GlobalStatisticAggregateDto>();
        }
    }
}