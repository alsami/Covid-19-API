using MediatR;

namespace Covid19Api.UseCases.Abstractions.Commands;

public sealed record AggregateGlobalStatisticsCommand(int Month, int Year) : IRequest;