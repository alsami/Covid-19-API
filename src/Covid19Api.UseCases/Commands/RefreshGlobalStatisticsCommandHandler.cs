using System.Threading;
using System.Threading.Tasks;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.Services.Abstractions.Parser;
using Covid19Api.UseCases.Abstractions.Commands;
using MediatR;

namespace Covid19Api.UseCases.Commands
{
    public class RefreshGlobalStatisticsCommandHandler : IRequestHandler<RefreshGlobalStatisticsCommand>
    {
        private readonly IGlobalStatisticsRepository globalStatisticsRepository;
        private readonly IGlobalStatisticsParser globalStatisticsParser;

        public RefreshGlobalStatisticsCommandHandler(IGlobalStatisticsRepository globalStatisticsRepository,
            IGlobalStatisticsParser globalStatisticsParser)
        {
            this.globalStatisticsRepository = globalStatisticsRepository;
            this.globalStatisticsParser = globalStatisticsParser;
        }

        public async Task<Unit> Handle(RefreshGlobalStatisticsCommand request, CancellationToken cancellationToken)
        {
            var globalStatistics = this.globalStatisticsParser.Parse(request.Document, request.FetchedAt);

            await this.globalStatisticsRepository.StoreAsync(globalStatistics);

            return Unit.Value;
        }
    }
}