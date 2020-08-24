using Covid19Api.Presentation.Response;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries
{
    public class LoadLatestGlobalStatisticsQuery : IRequest<GlobalStatisticsDto>
    {
    }
}