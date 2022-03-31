using MediatR;

namespace Covid19Api.UseCases.Abstractions.Commands;

public sealed record AggregateCountryStatisticsCommand(IEnumerable<string> Countries, int Month, int Year) : IRequest;