using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Covid19Api.Grpc.CountryStatistics;
using Covid19Api.UseCases.Abstractions.Queries.CountryStatistics;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;

namespace Covid19Api.Endpoints.Grpc
{
    public class CountryStatisticsServiceGrpc : CountryStatisticsService.CountryStatisticsServiceBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public CountryStatisticsServiceGrpc(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public override async Task<CountryStatisticsGrpcMessage> LoadLatestCountryStatistics(Empty request,
            ServerCallContext context)
        {
            var query = new LoadLatestCountriesStatisticsQuery();
            var countryStatistics = await this.mediator.Send(query);
            return new CountryStatisticsGrpcMessage
            {
                CountryStatistics =
                {
                    this.mapper.Map<IEnumerable<CountryStatisticGrpcMessage>>(countryStatistics)
                }
            };
        }

        public override async Task<CountryStatisticsGrpcMessage> LoadHistoricalCountryStatistics(Empty request,
            ServerCallContext context)
        {
            var query = new LoadHistoricalCountriesStatisticsQuery(DateTime.UtcNow.Date.AddDays(-9));
            var countryStatistics = await this.mediator.Send(query);
            return new CountryStatisticsGrpcMessage
            {
                CountryStatistics =
                {
                    this.mapper.Map<IEnumerable<CountryStatisticGrpcMessage>>(countryStatistics)
                }
            };
        }

        public override async Task<CountryStatisticGrpcMessage> LoadLatestCountryStatisticForCountry(
            CountryStatisticsForCountryGrpcMessage request, ServerCallContext context)
        {
            var query = new LoadLatestStatisticsForCountryQuery(request.Country);
            var countryStatistic = await this.mediator.Send(query);

            return this.mapper.Map<CountryStatisticGrpcMessage>(countryStatistic);
        }

        public override async Task<CountryStatisticsGrpcMessage> LoadHistoricalCountryStatisticForCountry(
            CountryStatisticsForCountryGrpcMessage request,
            ServerCallContext context)
        {
            var query = new LoadHistoricalCountryStatisticsForCountryQuery(request.Country);
            var countryStatistics = await this.mediator.Send(query);
            return new CountryStatisticsGrpcMessage
            {
                CountryStatistics =
                {
                    this.mapper.Map<IEnumerable<CountryStatisticGrpcMessage>>(countryStatistics)
                }
            };
        }
    }
}