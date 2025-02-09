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

        public override async Task<IdReply> DeleteReservation(IdRequest request, ServerCallContext context)
        {
            try
            {
                var deleteResult = await _reservationRepository.DeleteAsync(long.Parse(request.Id));

                if (deleteResult <= 0)
                {
                    return CreateErrorIdReply("Ошибка при удалении брони");
                }

                var result = new IdReply() { State = ReplyState.Ok };
                result.Id = deleteResult.ToString();
                return result;
            }
            catch (Exception ex)
            {
                return CreateErrorIdReply(ex.Message);
            }
        }

        public override async Task<ReservationsReply> GetReservations(ReservationQueryRequest request, ServerCallContext context)
        {
            try
            {
                var getResult = await _reservationRepository.GetByQueryAsync(request.ToQuery().Compile());
                var result = new ReservationsReply() { State = ReplyState.Ok };
                result.Reservations.AddRange(getResult.Select(x => x.ToGrpc()));
                return result;
            }
            catch (Exception ex)
            {
                return CreateErrorReply(ex.Message);
            }
        }

        public override async Task<ReservationsReply> UpdateReservation(Reservation request, ServerCallContext context)
        {
            try
            {
                var getResult = await _reservationRepository.UpdateAsync(request.ToEntity());
                var result = new ReservationsReply() { State = ReplyState.Ok };
                result.Reservations.Add(getResult.ToGrpc());
                return result;
            }
            catch (Exception ex)
            {
                return CreateErrorReply(ex.Message);
            }
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
