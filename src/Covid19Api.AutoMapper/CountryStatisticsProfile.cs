using AutoMapper;
using Covid19Api.Domain;
using Covid19Api.Grpc.CountryStatistics;
using Covid19Api.Presentation.Response;
using Google.Protobuf.WellKnownTypes;

// ReSharper disable UnusedMember.Global

namespace Covid19Api.AutoMapper
{
    public class CountryStatisticsProfile : Profile
    {
        public CountryStatisticsProfile()
        {
            this.CreateMap<CountryStatistic, CountryStatisticDto>()
                .ConvertUsing(src => new CountryStatisticDto(src.Country, src.CountryCode, src.TotalCases,
                    src.NewCases, src.TotalDeaths, src.NewDeaths, src.RecoveredCases, src.ActiveCases, src.FetchedAt));

            this.CreateMap<CountryStatisticDto, CountryStatisticGrpcMessage>()
                .ConvertUsing(src => new CountryStatisticGrpcMessage
                {
                    Country = src.Country,
                    CountryCode = src.CountryCode,
                    ActiveCases = src.ActiveCases,
                    NewCases = src.NewCases,
                    NewDeaths = src.NewDeaths,
                    RecoveredCases = src.RecoveredCases,
                    TotalCases = src.TotalCases,
                    TotalDeaths = src.TotalDeaths,
                    FetchedAt = Timestamp.FromDateTime(src.FetchedAt),
                });
        }
    }
}