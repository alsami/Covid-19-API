using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Covid19Api.Presentation.Response;
using Covid19Api.Services.Abstractions.Loader;
using Covid19Api.UseCases.Abstractions.Queries.GlobalStatistics;
using MediatR;

namespace Covid19Api.UseCases.Queries.GlobalStatistics
{
    public class
        LoadLatestGlobalStatisticsQueryHandler : IRequestHandler<LoadLatestGlobalStatisticsQuery, GlobalStatisticDto>
    {
        private readonly IHtmlDocumentLoader htmlDocumentLoader;
        private readonly IMapper mapper;
        private readonly IGlobalStatisticsLoader globalStatisticsLoader;

        public LoadLatestGlobalStatisticsQueryHandler(IHtmlDocumentLoader htmlDocumentLoader, IMapper mapper,
            IGlobalStatisticsLoader globalStatisticsLoader)
        {
            this.htmlDocumentLoader = htmlDocumentLoader;
            this.mapper = mapper;
            this.globalStatisticsLoader = globalStatisticsLoader;
        }

        public async Task<GlobalStatisticDto> Handle(LoadLatestGlobalStatisticsQuery request,
            CancellationToken cancellationToken)
        {
            var fetchedAt = DateTime.UtcNow;

            var latest = await this.globalStatisticsLoader.ParseAsync(fetchedAt);

            return this.mapper.Map<GlobalStatisticDto>(latest);
        }
    }
}