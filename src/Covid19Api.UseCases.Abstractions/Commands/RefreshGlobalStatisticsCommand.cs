using System;
using HtmlAgilityPack;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Commands
{
    public class RefreshGlobalStatisticsCommand : IRequest
    {
        public RefreshGlobalStatisticsCommand(DateTime fetchedAt, HtmlDocument document)
        {
            this.FetchedAt = fetchedAt;
            this.Document = document;
        }

        public DateTime FetchedAt { get; }

        public HtmlDocument Document { get; }
    }
}