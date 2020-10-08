using System;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Commands
{
    public class RefreshGlobalStatisticsCommand : IRequest
    {
        public RefreshGlobalStatisticsCommand(DateTime fetchedAt)
        {
            this.FetchedAt = fetchedAt;
        }

        public DateTime FetchedAt { get; }
    }
}