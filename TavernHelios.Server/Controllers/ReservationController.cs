
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;
using static GrpcContract.ReservationService.ReservationService;
using TavernHelios.ReservationService.ApiCore.Extensions;
using GrpcContract.ReservationService;
using TavernHelios.ReservationService.APICore.DTOValues;

using TavernHelios.Utils.Reports;
using System.Net.Http;
using System.Text.Json;
using TavernHelios.MenuService.Common.DTOValues.Menu;
using TavernHelios.MenuService.Common.Enums;

namespace TavernHelios.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ILogger<ReservationController> _logger;
        ReservationServiceClient _grpcClient;
        private readonly HttpClient _httpClient;

        public ReservationController(
            ILogger<ReservationController> logger,
            ReservationServiceClient grpcClient // TODO прокинуть сюда не GRPC клиент, а объект бизнес-логики, который будет общаться с этим клиентом
            )
        {
            _logger = logger;
            _grpcClient = grpcClient;
            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _httpClient = new HttpClient(httpClientHandler);
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

        /// <summary>
        /// Экспортировать брони
        /// </summary>
        /// <returns>PDF</returns>
        [HttpGet("export")]
        [Produces("application/octet-stream")]
        public async Task<IActionResult> ExportReservations([FromQuery] string date, [FromQuery] string format = "pdf")
        {
            DateTime parsedDate;

            if (!DateTime.TryParse(date, out parsedDate))
            {
                return BadRequest("Некорректный формат даты.");
            }

            var beginDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(parsedDate.ToUniversalTime());
            var endDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(parsedDate.AddHours(23).AddMinutes(59).AddSeconds(59).ToUniversalTime());

            var reservationsReply = await _grpcClient.GetReservationsAsync(new ReservationQueryRequest
            {
                BeginDate = beginDate,
                EndDate = endDate
            });

            var reservations = reservationsReply.Reservations.Select(x => x.ToDto()).ToList();
            if (!reservations.Any())
                return NotFound("Нет бронирований на эту дату.");

            var dishData = await FetchDishesForReservations(reservations);

            var reservationsAt12 = reservations.Where(r => r.Date.Hour == 12).ToList();
            var reservationsAt13 = reservations.Where(r => r.Date.Hour == 13).ToList();

            var headers = new List<string> { "Сотрудник", "Суп", "Горячее", "Салаты", "Напитки" };

            List<List<string>> tableData12 = reservationsAt12.Select(res =>
                new List<string>
                {
            res.PersonId,
            GetDishByType(dishData, res.DishIds, DishType.Soup),
            GetDishByType(dishData, res.DishIds, DishType.HotDish),
            GetDishByType(dishData, res.DishIds, DishType.Salad),
            GetDishByType(dishData, res.DishIds, DishType.Drink)
                }).ToList();

            List<List<string>> tableData13 = reservationsAt13.Select(res =>
                new List<string>
                {
            res.PersonId,
            GetDishByType(dishData, res.DishIds, DishType.Soup),
            GetDishByType(dishData, res.DishIds, DishType.HotDish),
            GetDishByType(dishData, res.DishIds, DishType.Salad),
            GetDishByType(dishData, res.DishIds, DishType.Drink)
                }).ToList();

            var reportGenerator = ReportFactory.CreateReportGenerator(format, $"Брони на {parsedDate:yyyy-MM-dd}", headers);

            var fileStream = reportGenerator.GenerateMultiTableReport(
                new List<(string, List<List<string>>)>
                {
            ("Брони на 12:00", tableData12),
            ("Брони на 13:00", tableData13)
                });

            return File(fileStream, reportGenerator.GetMimeType(), $"Reservations_{parsedDate:yyyy-MM-dd}.{reportGenerator.GetFileExtension()}");
        }

        /// <summary>
        /// Возвращает название блюда определенного типа
        /// </summary>
        private string GetDishByType(Dictionary<string, DishValue> dishes, List<string> dishIds, DishType type)
        {
            var selectedDishes = dishIds
                .Where(dishId => dishes.ContainsKey(dishId) && dishes[dishId].DishType == type)
                .Select(dishId => dishes[dishId].Name)
                .ToList();

            return selectedDishes.Any() ? string.Join(", ", selectedDishes) : "—";
        }

        private async Task<Dictionary<string, DishValue>> FetchDishesForReservations(IEnumerable<ReservationValue> reservations)
        {
            var dishDict = new Dictionary<string, DishValue>();
            var dishIds = reservations.SelectMany(r => r.DishIds).Distinct().ToList();

            foreach (var dishId in dishIds)
            {
                var response = await _httpClient.GetAsync($"https://localhost:5555/api/dish/{dishId}"); // ✅ HTTP-запрос
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var dish = JsonSerializer.Deserialize<DishValue>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (dish != null)
                        dishDict[dish.Id] = dish;
                }
            }

            return dishDict;
        }
    }
}
