using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using TavernHelios.Server.Services.ReservedLayoutService;

namespace TavernHelios.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservedLayoutController : ControllerBase
    {
        private readonly ILogger<ReservedLayoutController> _logger;
        ReservedLayoutService _service;

        public ReservedLayoutController(
            ILogger<ReservedLayoutController> logger,
            ReservedLayoutService service //Наконец-то БИЗНЕС ЛОГИКА на бэкенде
            )
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// Получить схему зала с пометками о бронировании
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType<ReservedLayoutValue>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Получить схему зала с пометками о бронировании")]
        public async Task<IActionResult> GetReservedLayout([FromQuery] ReservedLayoutRequest queryRequest)
        {
            try
            {
                var reservedLayout = await _service.GetLayoutForReservationDate(
               queryRequest.ReservationDateTime,
               queryRequest.LayoutId);

                return Ok(reservedLayout);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    [Description("Запрос для получения информации о забронированных местах на конкретную дату и схему")]
    public class ReservedLayoutRequest
    {
        [Description("Дата и время бронирования для фильтрации")]
        public DateTime ReservationDateTime { get; set; } = DateTime.MinValue;

        [Description("Идентификатор схемы зала, для которой запрашиваются данные")]
        public string LayoutId { get; set; } = string.Empty;
    }
}
