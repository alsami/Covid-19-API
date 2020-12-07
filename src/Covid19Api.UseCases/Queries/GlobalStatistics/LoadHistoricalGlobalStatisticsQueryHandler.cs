using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Covid19Api.Presentation.Response;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.UseCases.Abstractions.Queries.GlobalStatistics;
using MediatR;

namespace Covid19Api.UseCases.Queries.GlobalStatistics
{
    public class
        LoadHistoricalGlobalStatisticsQueryHandler : IRequestHandler<LoadHistoricalGlobalStatisticsQuery,
            IEnumerable<GlobalStatisticDto>>
    {
        private readonly IGlobalStatisticsReadRepository globalStatisticsReadRepository;
        private readonly IMapper mapper;

        public LoadHistoricalGlobalStatisticsQueryHandler(IGlobalStatisticsReadRepository globalStatisticsReadRepository,
            IMapper mapper)
        {
            this.globalStatisticsReadRepository = globalStatisticsReadRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<GlobalStatisticDto>> Handle(LoadHistoricalGlobalStatisticsQuery request,
            CancellationToken cancellationToken)
        {
            var latestActiveCaseStats = await this.globalStatisticsReadRepository.HistoricalAsync(request.MinFetchedAt);

            return this.mapper.Map<IEnumerable<GlobalStatisticDto>>(latestActiveCaseStats);
        }
    }
}