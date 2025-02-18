
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;
using TavernHelios.MenuService.Common.DTOValues.Menu;
using static GrpcContract.MenuService.MenuService;
using TavernHelios.MenuService.Common.Extensions;
using GrpcContract.MenuService;
using TavernHelios.MenuService.APICore.DTOValues.Menu;

namespace TavernHelios.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuScheduleController : ControllerBase
    {
        private readonly ILogger<MenuController> _logger;
        MenuServiceClient _grpcClient;

        public MenuScheduleController(
            ILogger<MenuController> logger,
            MenuServiceClient grpcClient // TODO прокинуть сюда не GRPC клиент, а объект бизнес-логики, который будет общаться с этим клиентом
            )
        {
            _logger = logger;
            _grpcClient = grpcClient;

        }

        /// <summary>
        /// Получить единицы расписания по условию
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType<IEnumerable<MenuScheduleReplyValue>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Получить одну или несколько записей расписания, удовлетворяющих условию")]
        public async Task<IActionResult> GetByConditionAsync([FromQuery] MenuScheduleQueryRequestValue queryRequest)
        {
            var replySchedule = await _grpcClient.GetMenuSchedulesAsync(queryRequest.ToGrpc());

            var schedules = replySchedule.MenusSchedules.Select(x => x.ToDto());

            return Ok(schedules);
        }

        /// <summary>
        /// Создать единицу расписания
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType<MenuScheduleReplyValue>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Создать запись в расписании")]
        public async Task<IActionResult> CreateAsync([FromBody] MenuScheduleCreateValue scheduleValue)
        {
            var addResult = await _grpcClient.AddMenuScheduleAsync(scheduleValue.ToGrpc());
            var schedule = addResult.MenusSchedules.FirstOrDefault();

            if (schedule == null || addResult.State != GrpcContract.ReplyState.Ok)
                return BadRequest(addResult.Messages);

            var scheduleModel = schedule.ToDto();

            //TODO: correct URI
            return Created("api/v1/menu", scheduleModel);
        }

        /// <summary>
        /// Редактировать расписание
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType<MenuValue>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Редактировать запись в расписании")]
        public async Task<IActionResult> UpdateAsync([FromBody] MenuScheduleUpdateValue scheduleValue)
        {
            var updateResult = await _grpcClient.UpdateMenuScheduleAsync(scheduleValue.ToGrpc());
            var schedule = updateResult.MenusSchedules.FirstOrDefault();

            if (schedule == null || updateResult.State != GrpcContract.ReplyState.Ok)
                return BadRequest(updateResult.Messages);

            var scheduleModel = schedule.ToDto();

            //TODO: correct URI
            return Ok(scheduleModel);
        }

        /// <summary>
        /// Удалить меню
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Удалить запись в расписании")]
        public async Task<IActionResult> DeleteMenuAsync([FromBody] string scheduleId)
        {
            var deleteResult = await _grpcClient.DeleteMenuScheduleAsync(new GrpcContract.IdRequest() { Id = scheduleId});

            if (string.IsNullOrEmpty(deleteResult.Id)  || deleteResult.State != GrpcContract.ReplyState.Ok)
                return BadRequest(deleteResult.Messages);

            //TODO: correct URI
            return Ok(deleteResult.Id);
        }
    }
}
