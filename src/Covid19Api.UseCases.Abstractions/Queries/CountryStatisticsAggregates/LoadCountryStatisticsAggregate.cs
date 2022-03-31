using Covid19Api.Presentation.Response;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries.CountryStatisticsAggregates;

public sealed record LoadCountryStatisticsAggregate
    (string Country, int Month, int Year) : IRequest<CountryStatisticAggregateDto?>;