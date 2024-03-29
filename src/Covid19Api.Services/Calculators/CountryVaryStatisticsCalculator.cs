using Covid19Api.Domain;
using Covid19Api.Presentation.Response;
using Covid19Api.Services.Abstractions.Calculators;

namespace Covid19Api.Services.Calculators;

public class CountryVaryStatisticsCalculator : ICountryVaryStatisticsCalculator
{
    public IEnumerable<CountryVaryStatisticContainerDto> Calculate(IEnumerable<CountryStatistic> countryStatistics)
    {
        var countryStatisticsByFetchedAt = countryStatistics
            .OrderBy(statistic => statistic.FetchedAt)
            .GroupBy(statistic => statistic.FetchedAt)
            .ToList();

        for (var i = 0; i < countryStatisticsByFetchedAt.Count; i++)
        {
            var currentGroup = countryStatisticsByFetchedAt.ElementAt(i);

            if (i == 0)
            {
                continue;
            }

            var previousGroup = countryStatisticsByFetchedAt.ElementAt(i - 1);

            yield return new CountryVaryStatisticContainerDto(currentGroup.Key.Date, Calculate(currentGroup.ToList(), previousGroup.ToList()));
        }
    }

    private static IEnumerable<CountryVaryStatisticDto> Calculate(IReadOnlyList<CountryStatistic> current,
        IReadOnlyList<CountryStatistic> previous)
    {
        var currentTotalSum = current.Sum(c => c.TotalCases);
        var previousTotalSum = previous.Sum(c => c.TotalCases);
        yield return new CountryVaryStatisticDto(VaryKeys.Total,
            CalculateVary(currentTotalSum, previousTotalSum), previousTotalSum, currentTotalSum);

        var currentNewSum = current.Sum(c => c.NewCases);
        var previousNewSum = previous.Sum(c => c.NewCases);
        yield return new CountryVaryStatisticDto(VaryKeys.New,
            CalculateVary(currentNewSum, previousNewSum), previousNewSum, currentNewSum);

        var currentActiveSum = current.Sum(c => c.ActiveCases);
        var previousActiveSum = previous.Sum(c => c.ActiveCases);
        yield return new CountryVaryStatisticDto(VaryKeys.Active,
            CalculateVary(currentActiveSum, previousActiveSum), previousActiveSum, currentActiveSum);

        var currentDeathsSum = current.Sum(c => c.TotalDeaths);
        var previousDeathsSum = previous.Sum(c => c.TotalDeaths);
        yield return new CountryVaryStatisticDto(VaryKeys.Deaths,
            CalculateVary(currentDeathsSum, previousDeathsSum), previousDeathsSum, currentDeathsSum);

        var currentNewDeathsSum = current.Sum(c => c.NewDeaths);
        var previousNewDeathsSum = previous.Sum(c => c.NewDeaths);
        yield return new CountryVaryStatisticDto(VaryKeys.NewDeaths,
            CalculateVary(currentNewDeathsSum, previousNewDeathsSum), previousNewDeathsSum, currentNewDeathsSum);

        var currentRecoveredSum = current.Sum(c => c.RecoveredCases);
        var previousRecoveredSum = previous.Sum(c => c.RecoveredCases);
        yield return new CountryVaryStatisticDto(VaryKeys.Recovered,
            CalculateVary(currentRecoveredSum, previousRecoveredSum), previousRecoveredSum, currentRecoveredSum);
    }

    private static double? CalculateVary(int current, int previous)
    {
        var value = current > previous
            ? CalculateIncrease(current, previous)
            : CalculateDecrease(current, previous);

        if (double.IsInfinity(value) || double.IsNaN(value))
        {
            return null;
        }

        return value;
    }

    private static double CalculateIncrease(int current, int previous)
    {
        var difference = current - previous;
        return difference / (double) current * 100;
    }

    private static double CalculateDecrease(int current, int previous)
    {
        var difference = previous - current;
        var decrease = difference / (double) previous * 100;

        return decrease * -1;
    }
}