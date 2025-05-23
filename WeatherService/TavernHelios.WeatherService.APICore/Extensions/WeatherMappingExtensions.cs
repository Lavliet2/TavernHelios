using GrpcContract.WeatherService;
using TavernHelios.WeatherService.APICore.DTOValues;

namespace TavernHelios.WeatherService.APICore.Extensions;

public static class WeatherMappingExtensions
{
    public static WeatherRequest ToGrpc(this WeatherRequestValue dto)
    {
        return new WeatherRequest { City = dto.City };
    }
    public static WeatherReplyDto ToDto(this WeatherReply reply)
    {
        return new WeatherReplyDto
        {
            City = reply.City,
            TodayDate = reply.TodayDate,
            TomorrowDate = reply.TomorrowDate,
            AfterTomorrowDate = reply.AfterTomorrowDate,
            Today = reply.Today.ToList(),
            Tomorrow = reply.Tomorrow.ToList(),
            AfterTomorrow = reply.AfterTomorrow.ToList(),
            TodaySummary = reply.TodaySummary,
            TomorrowSummary = reply.TomorrowSummary,
            AfterTomorrowSummary = reply.AfterTomorrowSummary,
            State = reply.State,
            Messages = reply.Messages.ToList()
        };
    }

    public static WeatherReply ToGrpc(this WeatherReplyDto dto)
    {
        return new WeatherReply
        {
            City = dto.City,
            TodayDate = dto.TodayDate,
            TomorrowDate = dto.TomorrowDate,
            AfterTomorrowDate = dto.AfterTomorrowDate,
            Today = { dto.Today },
            Tomorrow = { dto.Tomorrow },
            AfterTomorrow = { dto.AfterTomorrow },
            TodaySummary = dto.TodaySummary,
            TomorrowSummary = dto.TomorrowSummary,
            AfterTomorrowSummary = dto.AfterTomorrowSummary,
            State = dto.State,
            Messages = { dto.Messages }
        };
    }

}
