using TavernHelios.MenuService.ApiCore.Interfaces;
using TavernHelios.MenuService.ApiCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using MongoRepositories.Entities;
using TavernHelios.MenuService.ApiCore.DTOValues.Menu;
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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation("Получить все блюда")]
        public async Task<ActionResult<IEnumerable<DishValue>>> GetAllDishesAsync()
        {
            var allDishes = await _grpcClient.GetAllDishesAsync(new GrpcContract.EmptyRequest());

            if (allDishes.State != GrpcContract.ReplyState.Ok)
            {
                return BadRequest(allDishes.Messages);
            }

            var values = allDishes?.Dishes.Select(x => x.ToDto());

            return Ok(values);
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
            var dishReply = await _grpcClient.GetDishAsync(new GrpcContract.IdRequest() { Id = id });
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
