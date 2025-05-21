using GrpcContract.WeatherService;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TavernHelios.WeatherService.APICore.DTOValues;
using TavernHelios.WeatherService.APICore.Extensions;
using static GrpcContract.WeatherService.WeatherService;

namespace TavernHelios.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly WeatherServiceClient _weatherClient;

    public WeatherController(WeatherServiceClient weatherClient)
    {
        _weatherClient = weatherClient;
    }

    [HttpGet]
    [SwaggerOperation("Получить прогноз погоды по городу")]
    public async Task<IActionResult> Get([FromQuery] WeatherRequestValue request)
    {
        var grpcRequest = request.ToGrpc();
        var reply = await _weatherClient.GetWeatherForecastAsync(grpcRequest);

        if (reply.State != GrpcContract.ReplyState.Ok)
            return BadRequest(reply.Messages);

        return Ok(reply);
    }
}
