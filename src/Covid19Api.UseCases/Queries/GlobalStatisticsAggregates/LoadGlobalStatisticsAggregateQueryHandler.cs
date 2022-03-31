using AutoMapper;
using Covid19Api.Presentation.Response;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.UseCases.Abstractions.Queries.GlobalStatisticsAggregates;
using MediatR;

namespace Covid19Api.UseCases.Queries.GlobalStatisticsAggregates;

public class
    LoadGlobalStatisticsAggregateQueryHandler : IRequestHandler<LoadGlobalStatisticsAggregate,
        GlobalStatisticAggregateDto?>
{
    private readonly IMapper mapper;
    private readonly IGlobalStatisticsAggregatesReadRepository globalStatisticsAggregatesReadRepository;

    public LoadGlobalStatisticsAggregateQueryHandler(IMapper mapper,
        IGlobalStatisticsAggregatesReadRepository globalStatisticsAggregatesReadRepository)
    {
        this.mapper = mapper;
        this.globalStatisticsAggregatesReadRepository = globalStatisticsAggregatesReadRepository;
    }

    public async Task<GlobalStatisticAggregateDto?> Handle(LoadGlobalStatisticsAggregate request,
        CancellationToken cancellationToken)
    {
        var aggregate = await this.globalStatisticsAggregatesReadRepository.FindAsync(request.Month, request.Year);

        return aggregate is null
            ? null
            : this.mapper.Map<GlobalStatisticAggregateDto>(aggregate);
    }
}