using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Covid19Api.Presentation.Response;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.UseCases.Abstractions.Queries.GlobalStatisticsAggregates;
using MediatR;

namespace Covid19Api.UseCases.Queries.GlobalStatisticsAggregates
{
    public class
        LoadGlobalStatisticsAggregateQueryHandler : IRequestHandler<LoadGlobalStatisticsAggregate,
            GlobalStatisticAggregateDto?>
    {
        private readonly IMapper mapper;
        private readonly IGlobalStatisticsAggregatesRepository globalStatisticsAggregatesRepository;

        public LoadGlobalStatisticsAggregateQueryHandler(IMapper mapper,
            IGlobalStatisticsAggregatesRepository globalStatisticsAggregatesRepository)
        {
            this.mapper = mapper;
            this.globalStatisticsAggregatesRepository = globalStatisticsAggregatesRepository;
        }

        public async Task<GlobalStatisticAggregateDto?> Handle(LoadGlobalStatisticsAggregate request,
            CancellationToken cancellationToken)
        {
            var aggregate = await this.globalStatisticsAggregatesRepository.FindAsync(request.Month, request.Year);

            return aggregate is null
                ? null
                : this.mapper.Map<GlobalStatisticAggregateDto>(aggregate);
        }
    }
}