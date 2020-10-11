using System.Threading;
using System.Threading.Tasks;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.Services.Abstractions.Loader;
using Covid19Api.UseCases.Abstractions.Commands;
using MediatR;

namespace Covid19Api.UseCases.Commands
{
    public class RefreshGlobalStatisticsCommandHandler : IRequestHandler<RefreshGlobalStatisticsCommand>
    {
        private readonly IGlobalStatisticsRepository globalStatisticsRepository;
        private readonly IGlobalStatisticsLoader globalStatisticsLoader;

        public RefreshGlobalStatisticsCommandHandler(IGlobalStatisticsRepository globalStatisticsRepository,
            IGlobalStatisticsLoader globalStatisticsLoader)
        {
            this.globalStatisticsRepository = globalStatisticsRepository;
            this.globalStatisticsLoader = globalStatisticsLoader;
        }

        public async Task<Unit> Handle(RefreshGlobalStatisticsCommand request, CancellationToken cancellationToken)
        {
            var globalStatistics = await this.globalStatisticsLoader.ParseAsync(request.FetchedAt);

            await this.globalStatisticsRepository.StoreAsync(globalStatistics);

            return Unit.Value;
        }
    }
}