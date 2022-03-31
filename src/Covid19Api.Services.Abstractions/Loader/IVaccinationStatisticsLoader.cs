using Covid19Api.Domain;

namespace Covid19Api.Services.Abstractions.Loader;

public interface IVaccinationStatisticsLoader
{
    Task<VaccinationStatistic[]> LoadAsync();
}