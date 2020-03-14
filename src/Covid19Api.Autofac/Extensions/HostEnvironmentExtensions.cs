using Microsoft.Extensions.Hosting;

namespace Covid19Api.AutoMapper.Extensions
{
    public static class HostEnvironmentExtensions
    {
        public static bool IsAzure(this IHostEnvironment hostingEnvironment)
            => hostingEnvironment.EnvironmentName == "Azure";
    }
}