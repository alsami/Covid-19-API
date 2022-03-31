using AutoMapper;
using Covid19Api.Domain;
using Covid19Api.Presentation.Response;

namespace Covid19Api.AutoMapper;

// ReSharper disable once UnusedType.Global
public class VaccinationStatisticProfile : Profile
{
    public VaccinationStatisticProfile()
    {
        this.CreateMap<VaccinationStatistic, VaccinationStatisticDto>()
            .ConvertUsing<VaccinationStatisticToDtoTypeConverter>();
    }
}