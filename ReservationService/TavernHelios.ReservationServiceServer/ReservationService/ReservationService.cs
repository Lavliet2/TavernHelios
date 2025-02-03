using TavernHelios.ReservationService.ApiCore.Interfaces;
using Grpc.Core;
using GrpcContract;
using Microsoft.Extensions.Options;
using TavernHelios.GrpcCommon.Settings;
using static GrpcContract.ReservationService.ReservationService;
using GrpcContract.ReservationService;
using TavernHelios.ReservationService.ApiCore.Extensions;
using TavernHelios.ReservationService.APICore.Entities;
using GrpcContract.MenuService;

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

        public override async Task<ReservationsReply> AddReservation(Reservation request, ServerCallContext context)
        {
            try
            {
                var addResult = await _reservationRepository.CreateAsync(request.ToEntity());

                if (addResult == null)
                {
                    return CreateErrorReply("Ошибка при добавлении брони");
                }

                var result = new ReservationsReply() { State = ReplyState.Ok };
                result.Reservations.Add(addResult.ToGrpc());
                return result;
            }
            catch (Exception ex)
            {
                return CreateErrorReply(ex.Message);
            }
        }

        public override Task<IdReply> DeleteReservation(IdRequest request, ServerCallContext context)
        {
            //TODO:
            throw new NotImplementedException();
        }

        public override async Task<ReservationsReply> GetReservations(ReservationQueryRequest request, ServerCallContext context)
        {
            try
            {
                var addResult = await _reservationRepository.GetAllAsync();

                if (addResult == null)
                {
                    return CreateErrorReply("Ошибка при получении блюд из БД");
                }

                var result = new ReservationsReply() { State = ReplyState.Ok };
                result.Reservations.AddRange(addResult.Select(x => x.ToGrpc()));
                return result;
            }
            catch (Exception ex)
            {
                return CreateErrorReply(ex.Message);
            }
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

        private ReservationsReply CreateErrorReply(string message)
        {
            var reply = new ReservationsReply()
            {
                State = ReplyState.Error
            };
            reply.Messages.Add(message);
            return reply;
        }
    }
}
