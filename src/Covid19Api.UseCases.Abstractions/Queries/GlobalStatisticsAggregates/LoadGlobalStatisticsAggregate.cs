using Covid19Api.Presentation.Response;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries.GlobalStatisticsAggregates
{
    public sealed record LoadGlobalStatisticsAggregate(int Month, int Year) : IRequest<GlobalStatisticAggregateDto?>;
}