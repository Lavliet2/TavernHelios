using GrpcContract.WeatherService;

namespace TavernHelios.WeatherServiceServer.WeatherService
{
    public interface IWeatherParser
    {
        WeatherReply ParseWeather(string json);
    }
}
