using TavernHelios.MenuService.Common.DTOValues.Menu;
using TavernHelios.MenuService.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using static GrpcContract.MenuService.MenuService;
using Swashbuckle.AspNetCore.Annotations;

namespace MenuServiceServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuEditController : ControllerBase
    {
        private readonly ILogger<MenuEditController> _logger;
        private readonly MenuServiceClient _grpcClient;

        public MenuEditController(
            ILogger<MenuEditController> logger,
            MenuServiceClient grpcClient
            )
        {
            _logger = logger;
            _grpcClient = grpcClient;
        }

        /// <summary>
        /// Добавить блюдо в меню
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType<MenuValue>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Добавить блюдо в меню")]
        public async Task<IActionResult> AddDishToMenuAsync([FromBody] MenuEditRequest request)
        {
            var reply = await _grpcClient.AddDishToMenuAsync(
                new GrpcContract.MenuService.DishMenuMessage()
                {
                    MenuId = request.MenuId,
                    DishId = request.DishId
                });

            var menu = reply.Menus.FirstOrDefault();

            if(menu == null || reply.State != GrpcContract.ReplyState.Ok)
            {
                return BadRequest(reply.Messages);
            }
            return Ok(menu.ToDto());
            //var menuEntity = await GetMenuAsync(request.MenuId);
            //if (menuEntity == null)
            //    return BadRequest($"Меню с ID={request.MenuId} не найдено");

            //if (menuEntity.Dishes.Contains(request.DishId))
            //    return BadRequest($"Меню с ID={request.MenuId} не уже содержит блюдо с ID={request.DishId}");

            //var dishEntity = await GetDishAsync(request.DishId);
            //if (dishEntity == null)
            //    return BadRequest($"Блюдо с ID={request.DishId} не найдено");

            //menuEntity.Dishes.Add(request.DishId);

            //var menuResult = await _menuRepository.UpdateAsync(menuEntity);

            //if (menuResult == null)
            //    return BadRequest("Ошибка при изменении меню");

            //var menuModel = menuResult.ToDto();

            //return Ok(menuModel);
        }

        /// <summary>
        /// Удалить блюдо из меню
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType<MenuValue>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Удалить блюдо из меню")]
        public async Task<IActionResult> RemoveDishFromMenuAsync([FromBody] MenuEditRequest request)
        {
            var reply = await _grpcClient.RemoveDishFromMenuAsync(
               new GrpcContract.MenuService.DishMenuMessage()
               {
                   MenuId = request.MenuId,
                   DishId = request.DishId
               });

            var menu = reply.Menus.FirstOrDefault();

            if (menu == null || reply.State != GrpcContract.ReplyState.Ok)
            {
                return BadRequest(reply.Messages);
            }
            return Ok(menu.ToDto());
            //var menuEntity = await GetMenuAsync(request.MenuId);
            //if (menuEntity == null)
            //    return BadRequest($"Меню с ID={request.MenuId} не найдено");

            //if (!menuEntity.Dishes.Contains(request.DishId))
            //    return BadRequest($"Меню с ID={request.MenuId} не содержит блюдо с ID={request.DishId}");

            //var dishEntity = await GetDishAsync(request.DishId);
            //if (dishEntity == null)
            //    return BadRequest($"Блюдо с ID={request.DishId} не найдено");

            //menuEntity.Dishes.Remove(menuEntity.Dishes.First(x => x == request.DishId));

            //var menuResult = await _menuRepository.UpdateAsync(menuEntity);

            //if (menuResult == null)
            //    return BadRequest("Ошибка при изменении меню");

            //var menuModel = menuResult.ToDto();

            //return Ok(menuModel);
        }
    }
}
