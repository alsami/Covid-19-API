using AutoMapper;
using Covid19Api.Domain;
using Covid19Api.Grpc.GlobalStatistics;
using Covid19Api.Presentation.Response;
using Google.Protobuf.WellKnownTypes;

// ReSharper disable UnusedType.Global

// ReSharper disable UnusedMember.Global
namespace Covid19Api.AutoMapper
{
    public class GlobalStatisticsProfile : Profile
    {
        public GlobalStatisticsProfile()
        {
            this.CreateMap<GlobalStatistics, GlobalStatisticDto>()
                .ConvertUsing(src =>
                    new GlobalStatisticDto(src.Total, src.Recovered, src.Deaths, src.FetchedAt));

            this.CreateMap<GlobalStatisticDto, GlobalStatisticsGrpcMessage>()
                .ConvertUsing(src => new GlobalStatisticsGrpcMessage
                {
                    Active = src.Active,
                    Deaths = src.Deaths,
                    Recovered = src.Recovered,
                    Total = src.Total,
                    FetchedAt = Timestamp.FromDateTime(src.FetchedAt)
                });
        }
    }
}