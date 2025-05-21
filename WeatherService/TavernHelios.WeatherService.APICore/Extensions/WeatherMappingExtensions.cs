using GrpcContract.WeatherService;
using TavernHelios.WeatherService.APICore.DTOValues;

namespace TavernHelios.WeatherService.APICore.Extensions;

public static class WeatherMappingExtensions
{
    public static WeatherRequest ToGrpc(this WeatherRequestValue dto)
    {
        return new WeatherRequest { City = dto.City };
    }
}
