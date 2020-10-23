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
            IEnumerable<GlobalStatisticsDto>>
    {
        private readonly IGlobalStatisticsRepository globalStatisticsRepository;
        private readonly IMapper mapper;

        public LoadHistoricalGlobalStatisticsQueryHandler(IGlobalStatisticsRepository globalStatisticsRepository,
            IMapper mapper)
        {
            this.globalStatisticsRepository = globalStatisticsRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<GlobalStatisticsDto>> Handle(LoadHistoricalGlobalStatisticsQuery request,
            CancellationToken cancellationToken)
        {
            var latestActiveCaseStats = await this.globalStatisticsRepository.HistoricalAsync(request.MinFetchedAt);

            return this.mapper.Map<IEnumerable<GlobalStatisticsDto>>(latestActiveCaseStats);
        }
    }
}