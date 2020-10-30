using System;
using System.Threading.Tasks;
using AutoMapper;
using Covid19Api.Grpc.GlobalStatistics;
using Covid19Api.UseCases.Abstractions.Queries.GlobalStatistics;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;

namespace Covid19Api.Endpoints.Grpc
{
    public class GlobalStatisticsServiceGrpc : GlobalStatisticsService.GlobalStatisticsServiceBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public GlobalStatisticsServiceGrpc(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public override async Task<GlobalStatisticsGrpcMessage> LoadLatestGlobalStatistics(Empty request,
            ServerCallContext context)
        {
            var query = new LoadLatestGlobalStatisticsQuery();
            var globalStatistics = await this.mediator.Send(query);
            return this.mapper.Map<GlobalStatisticsGrpcMessage>(globalStatistics);
        }

        public override async Task<GlobalStatisticsHistoryGrpcMessage> LoadHistoricalGlobalStatistics(Empty request,
            ServerCallContext context)
        {
            var query = new LoadHistoricalGlobalStatisticsQuery(DateTime.UtcNow.Date.AddDays(-9));
            var globalStatistics = await this.mediator.Send(query);
            return new GlobalStatisticsHistoryGrpcMessage
            {
                GlobalStatistics =
                {
                    this.mapper.Map<RepeatedField<GlobalStatisticsGrpcMessage>>(globalStatistics)
                }
            };
        }
    }
}