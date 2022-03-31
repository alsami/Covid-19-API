using AutoMapper;
using Covid19Api.Domain;
using Covid19Api.Presentation.Response;

namespace Covid19Api.AutoMapper;

public class VaccinationStatisticToDtoTypeConverter : ITypeConverter<VaccinationStatistic, VaccinationStatisticDto>
{
    public VaccinationStatisticDto Convert(VaccinationStatistic source, VaccinationStatisticDto destination,
        ResolutionContext context)
    {
        var values = source.Values.Select(value => new VaccinationStatisticValueDto(value.LoggedAt,
            value.TotalVaccinations, value.PeopleVaccinated, value.PeopleFullyVaccinated,
            value.PeopleFullyVaccinatedPerHundred, value.DailyVaccinations,
            value.TotalVaccinationsPerHundred, value.PeopleVaccinatedPerHundred,
            value.DailyVaccinationsPerMillion));
        return new VaccinationStatisticDto(source.Country, source.CountyCode,values.OrderBy(value => value.LoggedAt).ToArray());
    }
}