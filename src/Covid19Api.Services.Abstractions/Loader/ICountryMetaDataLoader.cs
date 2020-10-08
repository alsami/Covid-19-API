using System.Threading.Tasks;
using Covid19Api.Services.Abstractions.Models;

namespace Covid19Api.Services.Abstractions.Loader
{
    public interface ICountryMetaDataLoader
    {
        Task<CountryMetaData[]> LoadCountryMetaDataByCountryAsync();
    }
}