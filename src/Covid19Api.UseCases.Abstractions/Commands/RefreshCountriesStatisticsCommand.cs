using System;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Commands
{
    public class RefreshCountriesStatisticsCommand : IRequest
    {
        public RefreshCountriesStatisticsCommand(DateTime fetchedAt)
        {
            this.FetchedAt = fetchedAt;
        }

        public DateTime FetchedAt { get; }
    }
}