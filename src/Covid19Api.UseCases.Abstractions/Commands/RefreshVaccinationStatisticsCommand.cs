using MediatR;

namespace Covid19Api.UseCases.Abstractions.Commands
{
    public record RefreshVaccinationStatisticsCommand : IRequest;
}