using Covid19Api.Presentation.Response;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries.Vaccinations;

public record LoadVaccinationStatisticsForCountryQuery(string CountryOrCountryCode) : IRequest<VaccinationStatisticDto>;