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
        /// �������� ����� � ����
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType<MenuValue>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("�������� ����� � ����")]
        public async Task<IActionResult> AddDishToMenuAsync([FromBody] MenuEditRequest request)
        {
            var menuEntity = await GetMenuAsync(request.MenuId);
            if (menuEntity == null)
                return BadRequest($"���� � ID={request.MenuId} �� �������");

            if(menuEntity.Dishes.Contains(request.DishId))
                return BadRequest($"���� � ID={request.MenuId} ��� �������� ����� � ID={request.DishId}");

            var dishEntity = await GetDishAsync(request.DishId);
            if (dishEntity == null)
                return BadRequest($"����� � ID={request.DishId} �� �������");

            menuEntity.Dishes.Add(request.DishId);

            var menuResult = await _menuRepository.UpdateAsync(menuEntity);

            if (menuResult == null)
                return BadRequest("������ ��� �������������� ����");

            var menuModel = menuResult.ToDto();

            return Ok(menuModel);
        }

        /// <summary>
        /// ������� ����� �� ����
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType<MenuValue>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("������� ����� �� ����")]
        public async Task<IActionResult> RemoveDishFromMenuAsync([FromBody] MenuEditRequest request)
        {
            var menuEntity = await GetMenuAsync(request.MenuId);
            if (menuEntity == null)
                return BadRequest($"���� � ID={request.MenuId} �� �������");

            if (!menuEntity.Dishes.Contains(request.DishId))
                return BadRequest($"���� � ID={request.MenuId} �� �������� ����� � ID={request.DishId}");

            var dishEntity = await GetDishAsync(request.DishId);
            if (dishEntity == null)
                return BadRequest($"����� � ID={request.DishId} �� �������");

            menuEntity.Dishes.Remove(menuEntity.Dishes.First(x => x == request.DishId));

            var menuResult = await _menuRepository.UpdateAsync(menuEntity);

            if (menuResult == null)
                return BadRequest("������ ��� �������������� ����");

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
