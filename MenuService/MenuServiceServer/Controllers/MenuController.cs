using APICore.DTOValues.Menu;
using APICore.Interfaces;
using APICore.Extensions;
using Microsoft.AspNetCore.Mvc;
using MongoRepositories.Entities;

namespace MenuServiceServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<MenuController> _logger;
        private readonly IRepository<MenuEntity> _menuRepository;
        private readonly IRepository<DishEntity> _dishRepository;

        public MenuController(
            ILogger<MenuController> logger,
            IRepository<MenuEntity> menuRepository,
            IRepository<DishEntity> dishRepository
            )
        {
            _logger = logger;
            _menuRepository = menuRepository;
            _dishRepository = dishRepository;

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MenuValue>>> GetAllMenusAsync()
        {
            var test = await _menuRepository.GetAllAsync();

            var values = test.Select(x => x.ToDto());

            return Ok(values);
        }

        /// <summary>
        /// Получить данные меню по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType<MenuValueFull>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMenuByIdAsync(string id)
        {
            var menu = await _menuRepository.GetByIdAsync(id);

            if (menu == null)
                return NotFound();

            var menuModel = await menu.ToFullDtoAsync(_dishRepository);

            return Ok(menuModel);
        }

        /// <summary>
        /// Создать меню
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType<MenuValue>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMenuAsync([FromBody] MenuValue menuValue)
        {
            var menuResult = await _menuRepository.CreateAsync(menuValue.ToEntity());

            if (menuResult == null)
                return BadRequest();

            var menuModel = menuResult.ToDto();

            //TODO: correct URI
            return Created("api/v1/menus", menuModel);
        }

        /// <summary>
        /// Изменить данные сотрудника
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType<MenuValue>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateMenuAsync([FromBody] MenuValue menuValue)
        {
            var menuResult = await _menuRepository.UpdateAsync(menuValue.ToEntity());

            if (menuResult == null)
                return BadRequest();

            var menuModel = menuResult.ToDto();

            return Ok(menuModel);
        }

        /// <summary>
        /// Удалить сотрудника
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteMenuAsync([FromBody] string menuId)
        {
            var menuResult = await _menuRepository.DeleteAsync(menuId);

            if (menuResult == string.Empty)
                return BadRequest();

            return NoContent();
        }
    }
}
