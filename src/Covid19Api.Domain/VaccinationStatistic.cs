// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Covid19Api.Domain;

public record VaccinationStatistic(string Country, string CountyCode, VaccinationStatisticValue[] Values);