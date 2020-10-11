using System.Threading;
using System.Threading.Tasks;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.Services.Abstractions.Loader;
using Covid19Api.UseCases.Abstractions.Commands;
using Covid19Api.UseCases.Filter;
using MediatR;

namespace Covid19Api.UseCases.Commands
{
    public class RefreshCountriesStatisticsCommandHandler : IRequestHandler<RefreshCountriesStatisticsCommand>
    {
        private readonly ICountryStatisticsRepository countryStatisticsRepository;
        private readonly ICountryStatisticsLoader countryStatisticsLoader;

        public RefreshCountriesStatisticsCommandHandler(ICountryStatisticsRepository countryStatisticsRepository,
            ICountryStatisticsLoader countryStatisticsLoader)
        {
            this.countryStatisticsRepository = countryStatisticsRepository;
            this.countryStatisticsLoader = countryStatisticsLoader;
        }

        public async Task<Unit> Handle(RefreshCountriesStatisticsCommand request, CancellationToken cancellationToken)
        {
            var countryStats =
                await this.countryStatisticsLoader.ParseAsync(request.FetchedAt, CountryStatsFilter.ValidOnly.Value);

            await this.countryStatisticsRepository.StoreManyAsync(countryStats!);

            return Unit.Value;
        }
    }
}