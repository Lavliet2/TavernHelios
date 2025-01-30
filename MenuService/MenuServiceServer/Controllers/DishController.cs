using APICore.Interfaces;
using APICore.Extensions;
using Microsoft.AspNetCore.Mvc;
using MongoRepositories.Entities;
using APICore.DTOValues.Menu;
using Swashbuckle.AspNetCore.Annotations;

namespace DishServiceServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishController : ControllerBase
    {
        private readonly ILogger<DishController> _logger;
        
        private readonly IRepository<DishEntity> _dishRepository;

        public DishController(
            ILogger<DishController> logger,
            IRepository<DishEntity> dishRepository
            )
        {
            _logger = logger;
            _dishRepository = dishRepository;

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation("�������� ��� �����")]
        public async Task<ActionResult<IEnumerable<DishValue>>> GetAllDishsAsync()
        {
            var test = await _dishRepository.GetAllAsync();

            var values = test.Select(x => x.ToDto());

            return Ok(values);
        }

        /// <summary>
        /// �������� ������ ����� �� Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType<DishValue>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("�������� ������ ����� �� Id")]
        public async Task<IActionResult> GetDishByIdAsync(string id)
        {
            var dish = await _dishRepository.GetByIdAsync(id);

            if (dish == null)
                return NotFound();

            return Ok(dish);
        }

        /// <summary>
        /// ������� �����
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType<DishValue>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("������� �����")]
        public async Task<IActionResult> CreateDishAsync([FromBody] DishValue dishValue)
        {
            var dishResult = await _dishRepository.CreateAsync(dishValue.ToEntity());

            if (dishResult == null)
                return BadRequest();

            var dishModel = dishResult.ToDto();

            //TODO: correct URI
            return Created("api/v1/dish", dishModel);
        }

        /// <summary>
        /// ������������� �����
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType<DishValue>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("������������� �����")]
        public async Task<IActionResult> UpdateDishAsync([FromBody] DishValue dishValue)
        {
            var dishResult = await _dishRepository.UpdateAsync(dishValue.ToEntity());

            if (dishResult == null)
                return BadRequest();

            var dishModel = dishResult.ToDto();

            return Ok(dishModel);
        }

        /// <summary>
        /// ������� �����
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("������� �����")]
        public async Task<IActionResult> DeleteDishAsync([FromBody] string dishId)
        {
            var dishResult = await _dishRepository.DeleteAsync(dishId);

            if (dishResult == string.Empty)
                return BadRequest();

            return NoContent();
        }
    }
}
