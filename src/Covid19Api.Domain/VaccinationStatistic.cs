// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Covid19Api.Domain;

public class VaccinationStatistic
{
    public VaccinationStatistic(string country, string countyCode, VaccinationStatisticValue[] values)
    {
        this.Country = country;
        this.CountyCode = countyCode;
        this.Values = values;
    }

    public string Country { get; private set; }

    public string CountyCode { get; private set; }

    public VaccinationStatisticValue[] Values { get; private set; }
}