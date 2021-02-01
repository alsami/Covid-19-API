using System;
using System.Collections.Generic;
using System.Linq;
using Covid19Api.Domain;
using Covid19Api.Presentation.Response;
using Covid19Api.Services.Calculators;
using FluentAssertions;
using Xunit;

namespace Covid19Api.Services.Tests
{
    public class CountryVaryStatisticsCalculatorTests
    {
        [Fact]
        public void VaryCalculationForSixValuesWithinTwoDays()
        {
            var statistics = GivenSixCountryStatisticsSplitIntoTwoDays();
            var res = WhenCalculatingVary(statistics);
            ThenCalculationShouldYieldExpectedValues(res);
        }

        private static IEnumerable<CountryStatistic> GivenSixCountryStatisticsSplitIntoTwoDays()
        {
            var dayOne = DateTime.UtcNow.Date;
            var dayTwo = dayOne.AddDays(-1);
            var statistics = new List<CountryStatistic>
            {
                new("Germany", "DE", 100, 10, 10, 1, 50, 50, dayOne),
                new("Italy", "IT", 100, 10, 10, 1, 50, 50, dayOne),
                new("France", "FR", 100, 10, 10, 1, 50, 50, dayOne),

                new("SomeCountry", "DE", 150, 5, 40, 2, 25, 75, dayTwo),
                new("SomeNotItaly", "IT", 150, 5, 40, 2, 25, 75, dayTwo),
                new("SomeMaybeFrance", "FR", 150, 5, 40, 2, 25, 75, dayTwo),
            };
            return statistics;
        }

        private static List<CountryVaryStatisticContainerDto> WhenCalculatingVary(
            IEnumerable<CountryStatistic> statistics)
        {
            var calculator = new CountryVaryStatisticsCalculator();
            var res = calculator.Calculate(statistics).ToList();
            return res;
        }

        private static void ThenCalculationShouldYieldExpectedValues(
            IReadOnlyList<CountryVaryStatisticContainerDto> res)
        {
            res.Count.Should().Be(2);
            var varyFirst = res[0].Vary.ToList();
            varyFirst.Count.Should().Be(6);
            var varySecond = res[1].Vary.ToList();
            varySecond.Count.Should().Be(6);
        }
    }
}