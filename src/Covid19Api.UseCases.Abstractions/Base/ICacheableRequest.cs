using Covid19Api.UseCases.Abstractions.Models;

namespace Covid19Api.UseCases.Abstractions.Base
{
    public interface ICacheableRequest
    {
        public CacheConfiguration GetCacheConfiguration();
    }
}