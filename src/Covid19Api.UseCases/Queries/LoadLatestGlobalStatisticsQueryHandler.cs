using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Covid19Api.Presentation.Response;
using Covid19Api.Services.Abstractions.Caching;
using Covid19Api.Services.Abstractions.Parser;
using Covid19Api.UseCases.Abstractions.Queries;
using MediatR;

namespace Covid19Api.UseCases.Queries
{
    public class
        LoadLatestGlobalStatisticsQueryHandler : IRequestHandler<LoadLatestGlobalStatisticsQuery, GlobalStatisticsDto>
    {
        private readonly IHtmlDocumentCache htmlDocumentCache;
        private readonly IMapper mapper;
        private readonly IGlobalStatisticsParser globalStatisticsParser;

        public LoadLatestGlobalStatisticsQueryHandler(IHtmlDocumentCache htmlDocumentCache, IMapper mapper,
            IGlobalStatisticsParser globalStatisticsParser)
        {
            this.htmlDocumentCache = htmlDocumentCache;
            this.mapper = mapper;
            this.globalStatisticsParser = globalStatisticsParser;
        }

        public async Task<GlobalStatisticsDto> Handle(LoadLatestGlobalStatisticsQuery request,
            CancellationToken cancellationToken)
        {
            var fetchedAt = DateTime.UtcNow;

            var latest = await this.globalStatisticsParser.ParseAsync(fetchedAt);

            return this.mapper.Map<GlobalStatisticsDto>(latest);
        }
    }
}