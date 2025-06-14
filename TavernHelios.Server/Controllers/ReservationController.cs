using GrpcContract.ReservationService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Swashbuckle.AspNetCore.Annotations;
using TavernHelios.ReservationService.ApiCore.Extensions;
using TavernHelios.ReservationService.APICore.DTOValues;
using TavernHelios.Server.Hubs;
using TavernHelios.Server.Services;
using static GrpcContract.ReservationService.ReservationService;

namespace TavernHelios.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ILogger<ReservationController> _logger;
        ReservationServiceClient _grpcClient;
        private readonly ReservationExportService _reservationExportService;
        private readonly IHubContext<ReservationHub> _hubContext;

        public ReservationController(
            ILogger<ReservationController> logger,
            ReservationServiceClient grpcClient, // TODO прокинуть сюда не GRPC клиент, а объект бизнес-логики, который будет общаться с этим клиентом
            ReservationExportService reservationExportService,
            IHubContext<ReservationHub> hubContext
            )
        {
            _logger = logger;
            _grpcClient = grpcClient;
            _reservationExportService = reservationExportService;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Получить меню по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType<ReservationValue>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Получить одну или несколько броней удовлетворяющих условию")]
        public async Task<IActionResult> GetReservationsByCondition([FromQuery] ReservationQueryRequestValue condition)
        {
            var replyReservation = await _grpcClient.GetReservationsAsync(condition.ToGrpc());

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
        public async Task<IActionResult> CreateReservationAsync([FromBody] ReservationCreateValue ReservationValue)
        {
            var ReservationResult = await _grpcClient.AddReservationAsync(ReservationValue.ToGrpc());
            var Reservation = ReservationResult.Reservations.FirstOrDefault();

            if (Reservation == null || ReservationResult.State != GrpcContract.ReplyState.Ok)
                return BadRequest(ReservationResult.Messages);

            var ReservationModel = Reservation.ToDto();

            await _hubContext.Clients.All.SendAsync("ReservationCreated", ReservationModel);

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

        [HttpGet("by-date")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation("Получить бронирования по дате")]
        public async Task<IActionResult> GetReservationsByDate([FromQuery] string date)
        {
            if (!DateTime.TryParse(date, out var parsedDate))
            {
                return BadRequest("Некорректный формат даты.");
            }

            var beginDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(parsedDate.ToUniversalTime());
            var endDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(parsedDate.AddHours(23).AddMinutes(59).AddSeconds(59).ToUniversalTime());

            var replyReservation = await _grpcClient.GetReservationsAsync(new ReservationQueryRequest
            {
                BeginDate = beginDate,
                EndDate = endDate
            });

            var reservations = replyReservation.Reservations.Select(x => x.ToDto()).ToList();

            if (!reservations.Any())
                return NotFound("Нет бронирований на эту дату.");

            return Ok(reservations);
        }

        /// <summary>
        /// Экспортировать брони
        /// </summary>
        /// <returns>PDF</returns>
        [HttpGet("export")]
        [Produces("application/octet-stream")]
        public async Task<IActionResult> ExportReservations([FromQuery] string date, [FromQuery] string format = "pdf")
        {
            if (!DateTime.TryParse(date, out var parsedDate))
            {
                return BadRequest("Некорректный формат даты.");
            }

            try
            {
                var fileStream = await _reservationExportService.ExportReservationsAsync(parsedDate, format);
                return File(fileStream, "application/pdf", $"Reservations_{parsedDate:yyyy-MM-dd}.{format}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
