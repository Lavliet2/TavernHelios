
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;
using static GrpcContract.LayoutService.LayoutService;

using GrpcContract.LayoutService;
using TavernHelios.LayoutService.APICore.DTOValues.Layout;
using TavernHelios.LayoutService.APICore.Extensions;
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
               queryRequest.RestaurantId,
               queryRequest.LayoutId);

                return Ok(reservedLayout);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class ReservedLayoutRequest
    {
        public DateTime ReservationDateTime { get; set; } = DateTime.MinValue;

        public string RestaurantId { get; set; } = string.Empty;

        public string? LayoutId { get; set; } = null;
    }
}
