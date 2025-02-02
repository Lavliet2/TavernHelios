using Microsoft.AspNetCore.Mvc;
using TavernHelios.Server.models.menu;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/[controller]")]
public class MenuProxyController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public MenuProxyController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("MenuServiceClient");
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation("Get all menu from API MenuService")]
    [HttpGet]
    public async Task<IActionResult> GetMenu()
    {
        try
        {
            var menu = await _httpClient.GetFromJsonAsync<List<Menu>>("api/Menu"); // ������ ����� BaseAddress

            if (menu == null || menu.Count == 0)
            {
                return NotFound("���� �� �������.");
            }
            return Ok(menu);
        }
        catch (HttpRequestException e)
        {
            return StatusCode(500, $"������ ������� ����: {e.Message}");
        }
    }


    /// <summary>
    /// Get dish by id from API MenuService
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation("Get dish by id from API MenuService")]
    [HttpGet("dish/{dishId}")]
    public async Task<IActionResult> GetDish(string dishId)
    {
        try
        {
            var dish = await _httpClient.GetFromJsonAsync<Dish>("api/Dish/" + dishId);

            if (dish == null)
            {
                return NotFound($"����� � ID {dishId} �� �������.");
            }
            return Ok(dish);
        }
        catch (HttpRequestException e)
        {
            return StatusCode(500, $"������ ��������� �����: {e.Message}");
        }
    }
}




