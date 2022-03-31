using Covid19Api.Repositories.Abstractions;
using Covid19Api.Services.Abstractions.Loader;
using Covid19Api.UseCases.Abstractions.Commands;
using MediatR;

namespace Covid19Api.UseCases.Commands;

public class RefreshVaccinationStatisticsCommandHandler : IRequestHandler<RefreshVaccinationStatisticsCommand>
{
    private readonly IVaccinationStatisticsLoader vaccinationStatisticsLoader;
    private readonly IVaccinationStatisticWriteRepository vaccinationStatisticWriteRepository;

    public RefreshVaccinationStatisticsCommandHandler(IVaccinationStatisticsLoader vaccinationStatisticsLoader, IVaccinationStatisticWriteRepository vaccinationStatisticWriteRepository)
    {
        this.vaccinationStatisticsLoader = vaccinationStatisticsLoader;
        this.vaccinationStatisticWriteRepository = vaccinationStatisticWriteRepository;
    }

    public async Task<Unit> Handle(RefreshVaccinationStatisticsCommand request, CancellationToken cancellationToken)
    {
        var vaccinationStatistics = await this.vaccinationStatisticsLoader.LoadAsync();
        if (vaccinationStatistics.Length == 0)
        {
            return Unit.Value;    
        }
            
        await this.vaccinationStatisticWriteRepository.StoreAsync(vaccinationStatistics);
        return Unit.Value;
    }
}