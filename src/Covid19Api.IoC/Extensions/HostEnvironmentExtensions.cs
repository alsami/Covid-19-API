using Microsoft.Extensions.Hosting;

namespace Covid19Api.IoC.Extensions;

public static class HostEnvironmentExtensions
{
    public static bool IsAzure(this IHostEnvironment hostingEnvironment)
        => hostingEnvironment.EnvironmentName == "Azure";

    public static bool IsContinuousIntegration(this IHostEnvironment hostEnvironment)
        => hostEnvironment.EnvironmentName == "CI";
}