using GrpcContract.LayoutService;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TavernHelios.LayoutService.APICore.DTOValues.Layout;
using TavernHelios.LayoutService.APICore.Extensions;
using static GrpcContract.LayoutService.LayoutService;

namespace TavernHelios.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LayoutController : ControllerBase
    {
        private readonly ILogger<LayoutController> _logger;
        LayoutServiceClient _grpcClient;

        public LayoutController(
            ILogger<LayoutController> logger,
            LayoutServiceClient grpcClient // TODO прокинуть сюда не GRPC клиент, а объект бизнес-логики, который будет общаться с этим клиентом
            )
        {
            _logger = logger;
            _grpcClient = grpcClient;

        }

        /// <summary>
        /// Получить одно или несколько меню по условию
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType<IEnumerable<LayoutValue>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Получить одну или несколько схем зала, удовлетворяющих условию")]
        public async Task<IActionResult> GetByConditionAsync([FromQuery] LayoutQueryRequestValue queryRequest)
        {
            var replyLayout = await _grpcClient.GetLayoutsAsync(queryRequest.ToGrpc());

            var Layouts = replyLayout.Layouts.Select(x => x.ToDto());

            return Ok(Layouts);
        }

        /// <summary>
        /// Создать меню
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType<LayoutValue>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Создать схему зала")]
        public async Task<IActionResult> CreateLayoutAsync([FromBody] AddLayoutRequestValue layoutValue)
        {
            var LayoutResult = await _grpcClient.AddLayoutAsync(layoutValue.ToGrpc());
            var Layout = LayoutResult.Layouts.FirstOrDefault();

            if (Layout == null || LayoutResult.State != GrpcContract.ReplyState.Ok)
                return BadRequest(LayoutResult.Messages);

            var LayoutModel = Layout.ToDto();

            //TODO: correct URI
            return Created("api/v1/Layouts", LayoutModel);
        }

        /// <summary>
        /// Редактировать меню
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType<LayoutValue>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Редактировать схему зала")]
        public async Task<IActionResult> UpdateLayoutAsync([FromBody] LayoutValue LayoutValue)
        {
            var LayoutResult = await _grpcClient.UpdateLayoutAsync(LayoutValue.ToGrpc());
            var Layout = LayoutResult.Layouts.FirstOrDefault();

            if (Layout == null || LayoutResult.State != GrpcContract.ReplyState.Ok)
                return BadRequest(LayoutResult.Messages);

            var LayoutModel = Layout.ToDto();

            return Ok(LayoutModel);
        }

        /// <summary>
        /// Удалить меню
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Удалить схему зала")]
        public async Task<IActionResult> DeleteLayoutAsync([FromBody] string LayoutId)
        {
            var LayoutResult = await _grpcClient.DeleteLayoutAsync(new GrpcContract.IdRequest() { Id = LayoutId });

            if (LayoutResult.State != GrpcContract.ReplyState.Ok)
                return BadRequest(LayoutResult.Messages);

            return NoContent();
        }
    }
}
