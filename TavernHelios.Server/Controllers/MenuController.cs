using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TavernHelios.MenuService.Common.DTOValues.Menu;
using TavernHelios.MenuService.Common.Extensions;
using static GrpcContract.MenuService.MenuService;

namespace TavernHelios.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly ILogger<MenuController> _logger;
        MenuServiceClient _grpcClient;

        public MenuController(
            ILogger<MenuController> logger,
            MenuServiceClient grpcClient // TODO прокинуть сюда не GRPC клиент, а объект бизнес-логики, который будет общаться с этим клиентом
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
        [ProducesResponseType<IEnumerable<MenuValue>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Получить одно или несколько меню, удовлетворяющих условию")]
        public async Task<IActionResult> GetByConditionAsync([FromQuery] MenuQueryRequestValue queryRequest)
        {
            var replyMenu = await _grpcClient.GetMenusAsync(queryRequest.ToGrpc());

            var menus = replyMenu.Menus.Select(x => x.ToDto());

            return Ok(menus);
        }

        /// <summary>
        /// Получить меню по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType<MenuValue>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Получить меню по Id (включая все блюда)")]
        public async Task<IActionResult> GetMenuByIdAsync(string id)
        {
            var query = new MenuQueryRequestValue() { MenuId = id };
            var replyMenu = await _grpcClient.GetMenusAsync(query.ToGrpc());

            var menu = replyMenu.Menus.FirstOrDefault();

            if (menu == null || replyMenu.State != GrpcContract.ReplyState.Ok)
                return NotFound(replyMenu.Messages);

            var dishesForMenuReply = await _grpcClient.GetAllDishesForMenuAsync(new GrpcContract.IdRequest() { Id = id });

            if (dishesForMenuReply == null || dishesForMenuReply.State != GrpcContract.ReplyState.Ok)
                return NotFound(dishesForMenuReply.Messages);

            var dishesForMenu = dishesForMenuReply.Dishes.Select(x => x.ToDto()).ToList();

            var menuModel = new MenuValueFull()
            {
                Id = id,
                Name = menu.Name,
                Dishes = menu.Dishes.ToList(),
                DishesFull = dishesForMenu,
                IsDeleted = menu.IsDeleted
            };

            return Ok(menuModel);
        }

        /// <summary>
        /// Создать меню
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType<MenuValue>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Создать меню")]
        public async Task<IActionResult> CreateMenuAsync([FromBody] MenuCreateValue menuValue)
        {
            var menuResult = await _grpcClient.AddMenuAsync(menuValue.ToGrpc());
            var menu = menuResult.Menus.FirstOrDefault();

            if (menu == null || menuResult.State != GrpcContract.ReplyState.Ok)
                return BadRequest(menuResult.Messages);

            var menuModel = menu.ToDto();

            //TODO: correct URI
            return Created("api/v1/menus", menuModel);
        }

        /// <summary>
        /// Редактировать меню
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType<MenuValue>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Редактировать меню")]
        public async Task<IActionResult> UpdateMenuAsync([FromBody] MenuValue menuValue)
        {
            var menuResult = await _grpcClient.UpdateMenuAsync(menuValue.ToGrpc());
            var menu = menuResult.Menus.FirstOrDefault();

            if (menu == null || menuResult.State != GrpcContract.ReplyState.Ok)
                return BadRequest(menuResult.Messages);

            var menuModel = menu.ToDto();

            return Ok(menuModel);
        }

        /// <summary>
        /// Удалить меню
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Удалить меню")]
        public async Task<IActionResult> DeleteMenuAsync([FromBody] string menuId)
        {
            var menuResult = await _grpcClient.DeleteMenuAsync(new GrpcContract.IdRequest() { Id = menuId });

            if (menuResult.State != GrpcContract.ReplyState.Ok)
                return BadRequest(menuResult.Messages);

            return NoContent();
        }
    }
}
