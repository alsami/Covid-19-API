using AutoMapper;
using Covid19Api.Presentation.Response;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.UseCases.Abstractions.Queries.GlobalStatistics;
using MediatR;

namespace Covid19Api.UseCases.Queries.GlobalStatistics;

public class
    LoadLatestGlobalStatisticsQueryHandler : IRequestHandler<LoadLatestGlobalStatisticsQuery, GlobalStatisticDto>
{
    private readonly IMapper mapper;
    private readonly IGlobalStatisticsReadRepository globalStatisticsReadRepository;

    public LoadLatestGlobalStatisticsQueryHandler(IMapper mapper, IGlobalStatisticsReadRepository globalStatisticsReadRepository)
    {
        this.mapper = mapper;
        this.globalStatisticsReadRepository = globalStatisticsReadRepository;
    }


    public async Task<GlobalStatisticDto> Handle(LoadLatestGlobalStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        var current = await this.globalStatisticsReadRepository.LoadCurrentAsync();

        return this.mapper.Map<GlobalStatisticDto>(current);
    }
}