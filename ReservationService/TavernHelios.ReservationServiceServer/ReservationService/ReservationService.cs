using TavernHelios.ReservationService.ApiCore.Interfaces;
using Grpc.Core;
using GrpcContract;
using Microsoft.Extensions.Options;
using TavernHelios.GrpcCommon.Settings;
using static GrpcContract.ReservationService.ReservationService;
using GrpcContract.ReservationService;
using TavernHelios.ReservationService.ApiCore.Extensions;
using TavernHelios.ReservationService.APICore.Entities;

namespace ReservationServiceServer.ReservationService
{
    /// <summary>
    /// Логика GRPC сервера, общение с репозиториями
    /// </summary>
    public class ReservationServiceApi : ReservationServiceBase
    {
        private readonly ILogger<ReservationServiceApi> _logger;
        private readonly IRepository<ReservationEntity> _reservationRepository;

        public ReservationServiceApi(
            ILogger<ReservationServiceApi> logger,
            IRepository<ReservationEntity> reservationRepository
            )
        {
            
            _logger = logger;
            _reservationRepository = reservationRepository;
        }

        public override Task<ReservationsReply> AddReservation(Reservation request, ServerCallContext context)
        {
            //TODO:
            throw new NotImplementedException();
        }

        public override Task<IdReply> DeleteReservation(IdRequest request, ServerCallContext context)
        {
            //TODO:
            throw new NotImplementedException();
        }

        public override Task<ReservationsReply> GetReservations(ReservationQueryRequest request, ServerCallContext context)
        {
            //TODO:
            throw new NotImplementedException();
        }

        public override Task<ReservationsReply> UpdateReservation(Reservation request, ServerCallContext context)
        {
            //TODO:
            throw new NotImplementedException();
        }

        private IdReply CreateErrorIdReply(string message)
        {
            var reply = new IdReply()
            {
                State = ReplyState.Error
            };
            reply.Messages.Add(message);
            return reply;
        }
    }
}
