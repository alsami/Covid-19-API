using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Covid19Api.Domain;

namespace Covid19Api.Services.Abstractions.Loader
{
    public interface ICountryStatisticsLoader
    {
        Task<IEnumerable<CountryStatistics?>> ParseAsync(DateTime fetchedAt,
            Func<CountryStatistics?, bool>? filter = null);
    }
}