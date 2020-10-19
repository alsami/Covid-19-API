using System.Threading.Tasks;
using Covid19Api.Domain;

namespace Covid19Api.Repositories.Abstractions
{
    public interface IGlobalStatisticsAggregatesRepository
    {
        Task StoreAsync(GlobalStatisticsAggregate globalStatisticsAggregate);

        Task<GlobalStatisticsAggregate?> FindAsync(int month, int year);
    }
}