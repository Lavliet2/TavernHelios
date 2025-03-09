using TavernHelios.MenuService.Common.Interfaces;
using TavernHelios.MenuService.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using TavernHelios.MenuService.Common.Entities;
using TavernHelios.MenuService.Common.DTOValues.Menu;
using static GrpcContract.MenuService.MenuService;
using Swashbuckle.AspNetCore.Annotations;
using GrpcContract.MenuService;

namespace DishServiceServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishController : ControllerBase
    {
        private readonly ILogger<DishController> _logger;        
        private readonly MenuServiceClient _grpcClient;

        public DishController(
            ILogger<DishController> logger,
            MenuServiceClient grpcClient
            )
        {
            _logger = logger;
            _grpcClient = grpcClient;
        }

        /// <summary>
        /// Получить одно или несколько блюд по условию
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType<IEnumerable<DishValue>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Получить одно или несколько блюд, удовлетворяющих условию")]
        public async Task<IActionResult> GetByConditionAsync([FromQuery] DishQueryRequestValue queryRequest)
        {
            var replyMenu = await _grpcClient.GetDishesAsync(queryRequest.ToGrpc());

            var menus = replyMenu.Dishes.Select(x => x.ToDto());

            return Ok(menus);
        }

        /// <summary>
        /// Получить блюдо по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType<DishValue>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Получить блюдо по Id")]
        public async Task<IActionResult> GetDishByIdAsync(string id)
        {
            var query = new DishQueryRequestValue() { DishId = id };
            var dishReply = await _grpcClient.GetDishesAsync(query.ToGrpc());

            var dish = dishReply.Dishes.FirstOrDefault();

            if (dish == null ||dishReply.State != GrpcContract.ReplyState.Ok)
                return NotFound(dishReply.Messages);

            return Ok(dish.ToDto());
        }

        /// <summary>
        /// Создать блюдо
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType<DishValue>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Создать блюдо")]
        public async Task<IActionResult> CreateDishAsync([FromBody] DishValue dishValue)
        {
            var dishResult = await _grpcClient.AddDishAsync(dishValue.ToGrpc());
            var dish = dishResult.Dishes.FirstOrDefault();

            if (dish == null || dishResult.State != GrpcContract.ReplyState.Ok)
                return BadRequest(dishResult.Messages);

            var dishModel = dish.ToDto();

            //TODO: correct URI
            return Created("api/v1/dish", dishModel);
        }

        /// <summary>
        /// Редактировать блюдо
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType<DishValue>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Редактировать блюдо")]
        public async Task<IActionResult> UpdateDishAsync([FromBody] DishValue dishValue)
        {
            var dishResult = await _grpcClient.UpdateDishAsync(dishValue.ToGrpc());
            var dish = dishResult.Dishes.FirstOrDefault();

            if (dish == null || dishResult.State != GrpcContract.ReplyState.Ok)
                return BadRequest(dishResult.Messages);

            var dishModel = dish.ToDto();

            return Ok(dishModel);
        }

        /// <summary>
        /// Удалить блюдо
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Удалить блюдо")]
        public async Task<IActionResult> DeleteDishAsync([FromBody] string dishId)
        {
            var dishResult = await _grpcClient.DeleteDishAsync(new GrpcContract.IdRequest() { Id = dishId });

            if (dishResult.State != GrpcContract.ReplyState.Ok)
                return BadRequest(dishResult.Messages);

            return NoContent();
        }
    }
}
