using GrpcContract;
using GrpcContract.WeatherService;

namespace TavernHelios.WeatherService.APICore.DTOValues;

public class WeatherReplyDto
{
    public string City { get; set; } = "";
    public string TodayDate { get; set; } = "";
    public string TomorrowDate { get; set; } = "";
    public string AfterTomorrowDate { get; set; } = "";

    public List<WeatherEntry> Today { get; set; } = new();
    public List<WeatherEntry> Tomorrow { get; set; } = new();
    public List<WeatherEntry> AfterTomorrow { get; set; } = new();

    public WeatherSummary? TodaySummary { get; set; }
    public WeatherSummary? TomorrowSummary { get; set; }
    public WeatherSummary? AfterTomorrowSummary { get; set; }

    public ReplyState State { get; set; }
    public List<string> Messages { get; set; } = new();
}
