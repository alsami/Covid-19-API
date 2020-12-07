using System.Collections.Generic;
using System.Threading.Tasks;
using Covid19Api.Domain;

namespace Covid19Api.Repositories.Abstractions
{
    public interface IGlobalStatisticsAggregatesReadRepository
    {
        Task<GlobalStatisticsAggregate?> FindAsync(int month, int year);

        Task<IList<GlobalStatisticsAggregate>> FindInYearAsync(int year);
    }
}