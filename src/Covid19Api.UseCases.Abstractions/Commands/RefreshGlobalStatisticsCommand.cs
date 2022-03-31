using MediatR;

namespace Covid19Api.UseCases.Abstractions.Commands;

public sealed record RefreshGlobalStatisticsCommand(DateTime FetchedAt) : IRequest;