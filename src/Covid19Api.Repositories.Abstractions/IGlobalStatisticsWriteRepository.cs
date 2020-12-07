using System.Threading.Tasks;
using Covid19Api.Domain;

namespace Covid19Api.Repositories.Abstractions
{
    public interface IGlobalStatisticsWriteRepository
    {
        Task StoreAsync(GlobalStatistics globalStatistics);
    }
}