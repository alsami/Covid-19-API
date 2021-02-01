using System.Collections.Generic;
using Covid19Api.Domain;
using Covid19Api.Presentation.Response;

namespace Covid19Api.Services.Abstractions.Calculators
{
    public interface ICountryVaryStatisticsCalculator
    {
        IEnumerable<CountryVaryStatisticContainerDto> Calculate(IEnumerable<CountryStatistic> countryStatistics);
    }
}