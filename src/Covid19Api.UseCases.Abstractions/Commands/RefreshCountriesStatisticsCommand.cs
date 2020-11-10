using System;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Commands
{
    public sealed record RefreshCountriesStatisticsCommand(DateTime FetchedAt) : IRequest;
}