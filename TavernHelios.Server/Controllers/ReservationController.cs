
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;
using static GrpcContract.ReservationService.ReservationService;
using TavernHelios.ReservationService.ApiCore.Extensions;
using GrpcContract.ReservationService;
using TavernHelios.ReservationService.APICore.DTOValues;

namespace TavernHelios.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ILogger<ReservationController> _logger;
        ReservationServiceClient _grpcClient;

        public ReservationController(
            ILogger<ReservationController> logger,
            ReservationServiceClient grpcClient // TODO прокинуть сюда не GRPC клиент, а объект бизнес-логики, который будет общаться с этим клиентом
            )
        {
            _logger = logger;
            _grpcClient = grpcClient;

        }

        /// <summary>
        /// Получить меню по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{condition}")]
        [ProducesResponseType<ReservationValue>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Получить бронь по Id")]
        public async Task<IActionResult> GetReservationsByCondition(ReservationQueryRequest condition)
        {
            var replyReservation = await _grpcClient.GetReservationsAsync(condition);

            var reservations= replyReservation.Reservations.Select(x => x.ToDto());

            return Ok(reservations);
        }

        /// <summary>
        /// Создать меню
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType<ReservationValue>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Создать бронь")]
        public async Task<IActionResult> CreateReservationAsync([FromBody] ReservationValue ReservationValue)
        {
            var ReservationResult = await _grpcClient.AddReservationAsync(ReservationValue.ToGrpc());
            var Reservation = ReservationResult.Reservations.FirstOrDefault();

            if (Reservation == null || ReservationResult.State != GrpcContract.ReplyState.Ok)
                return BadRequest(ReservationResult.Messages);

            var ReservationModel = Reservation.ToDto();

            //TODO: correct URI
            return Created("api/v1/Reservations", ReservationModel);
        }

        /// <summary>
        /// Редактировать меню
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType<ReservationValue>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Редактировать бронь")]
        public async Task<IActionResult> UpdateReservationAsync([FromBody] ReservationValue ReservationValue)
        {
            var ReservationResult = await _grpcClient.UpdateReservationAsync(ReservationValue.ToGrpc());
            var Reservation = ReservationResult.Reservations.FirstOrDefault();

            if (Reservation == null || ReservationResult.State != GrpcContract.ReplyState.Ok)
                return BadRequest(ReservationResult.Messages);

            var ReservationModel = Reservation.ToDto();

            return Ok(ReservationModel);
        }

        /// <summary>
        /// Удалить меню
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Удалить бронь")]
        public async Task<IActionResult> DeleteReservationAsync([FromBody] string ReservationId)
        {
            var ReservationResult = await _grpcClient.DeleteReservationAsync(new GrpcContract.IdRequest() { Id = ReservationId });

            if (ReservationResult.State != GrpcContract.ReplyState.Ok)
                return BadRequest(ReservationResult.Messages);

            return NoContent();
        }
    }
}
