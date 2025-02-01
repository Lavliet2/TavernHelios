using APICore.DTOValues.Menu;
using APICore.Interfaces;
using APICore.Extensions;
using Microsoft.AspNetCore.Mvc;
using MongoRepositories.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace MenuServiceServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuEditController : ControllerBase
    {
        private readonly ILogger<MenuController> _logger;
        private readonly IRepository<MenuEntity> _menuRepository;
        private readonly IRepository<DishEntity> _dishRepository;

        public MenuEditController(
            ILogger<MenuController> logger,
            IRepository<MenuEntity> menuRepository,
            IRepository<DishEntity> dishRepository
            )
        {
            _logger = logger;
            _menuRepository = menuRepository;
            _dishRepository = dishRepository;

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
            var menuEntity = await GetMenuAsync(request.MenuId);
            if (menuEntity == null)
                return BadRequest($"Меню с ID={request.MenuId} не найдено");

            if(menuEntity.Dishes.Contains(request.DishId))
                return BadRequest($"Меню с ID={request.MenuId} не уже содержит блюдо с ID={request.DishId}");

            var dishEntity = await GetDishAsync(request.DishId);
            if (dishEntity == null)
                return BadRequest($"Блюдо с ID={request.DishId} не найдено");

            menuEntity.Dishes.Add(request.DishId);

            var menuResult = await _menuRepository.UpdateAsync(menuEntity);

            if (menuResult == null)
                return BadRequest("Ошибка при изменении меню");

            var menuModel = menuResult.ToDto();

            return Ok(menuModel);
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
            var menuEntity = await GetMenuAsync(request.MenuId);
            if (menuEntity == null)
                return BadRequest($"Меню с ID={request.MenuId} не найдено");

            if (!menuEntity.Dishes.Contains(request.DishId))
                return BadRequest($"Меню с ID={request.MenuId} не содержит блюдо с ID={request.DishId}");

            var dishEntity = await GetDishAsync(request.DishId);
            if (dishEntity == null)
                return BadRequest($"Блюдо с ID={request.DishId} не найдено");

            menuEntity.Dishes.Remove(menuEntity.Dishes.First(x => x == request.DishId));

            var menuResult = await _menuRepository.UpdateAsync(menuEntity);

            if (menuResult == null)
                return BadRequest("Ошибка при изменении меню");

            var menuModel = menuResult.ToDto();

            return Ok(menuModel);
        }

        private async Task<MenuEntity> GetMenuAsync(string menuKey)
        {
            return await _menuRepository.GetByIdAsync(menuKey);
        }

        private async Task<DishEntity> GetDishAsync(string dishKey)
        {
            return await _dishRepository.GetByIdAsync(dishKey);
        }
    }
}
