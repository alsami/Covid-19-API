namespace Covid19Api.Presentation.Response;

public record VaccinationStatisticDto(string Country, string CountryCode, IEnumerable<VaccinationStatisticValueDto> Values);