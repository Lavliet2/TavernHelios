using Google.Protobuf.WellKnownTypes;
using GrpcContract.LayoutService;
using GrpcContract.ReservationService;
using TavernHelios.LayoutService.APICore.DTOValues.Layout;
using TavernHelios.LayoutService.APICore.Extensions;
using TavernHelios.ReservationService.ApiCore.Extensions;
using TavernHelios.ReservationService.APICore.DTOValues;
using static GrpcContract.LayoutService.LayoutService;
using static GrpcContract.ReservationService.ReservationService;

namespace TavernHelios.Server.Services.ReservedLayoutService;

public class ReservedLayoutService
{
    private readonly ILogger<ReservedLayoutService> _logger;
    private readonly LayoutServiceClient _layoutApiClient;
    private readonly ReservationServiceClient _reservationApiClient;

    public ReservedLayoutService(
        ILogger<ReservedLayoutService> logger,
        LayoutServiceClient layoutApiClient,
        ReservationServiceClient reservationApiClient)
    {
        _layoutApiClient = layoutApiClient;
        _logger = logger;
        _reservationApiClient = reservationApiClient;
    }

    /// <summary>
    /// Получить схему зала, где помечено, какие столы и стулья уже зарезервированы
    /// </summary>
    /// <param name="reservationDateTime"></param>
    /// <param name="RestaurantId"></param>
    /// <returns></returns>
    public async Task<ReservedLayoutValue> GetLayoutForReservationDate(DateTime reservationDateTime, string restaurantId, string? layoutId = null)
    {
        var reservations = await GetReservations(reservationDateTime, restaurantId);

        var layout = await GetLayout(reservationDateTime, restaurantId, layoutId);

        var reservedLayout = new ReservedLayoutValue(layout);
        reservedLayout.ReservationDateTime = reservationDateTime;
        
        foreach( var table in reservedLayout.Tables )
        {
            foreach(var seat in table.Seats)
            {
                var seatReservation = reservations.FirstOrDefault(x => x.SeatNumber == seat.Number);
                seat.IsReserved = seatReservation != null;
            }
            //Если все места заняты, то и стол тоже весь занят
            table.IsReserved = !table.Seats.Any(x => !x.IsReserved);
        }

        return reservedLayout;
    }

    private async Task<IEnumerable<ReservationValue>> GetReservations(DateTime reservationDateTime, string restaurantId)
    {
        //Получение бронирований
        ReservationQueryRequest reservationRequest = new();
        reservationRequest.BeginDate = reservationDateTime.ToUniversalTime().ToTimestamp();
        reservationRequest.EndDate = reservationDateTime.ToUniversalTime().ToTimestamp();
        reservationRequest.RestaurantId = restaurantId;
        reservationRequest.IsDeleted = false;
        var reservationsResult = await _reservationApiClient.GetReservationsAsync(reservationRequest);

        if (reservationsResult == null)
            throw new Exception("Не удалось получить список бронирований на дату");
        if (reservationsResult.State != GrpcContract.ReplyState.Ok)
            throw new Exception(string.Join("\n", reservationsResult.Messages));
        var reservations = reservationsResult.Reservations.Select(x => x.ToDto());
        return reservations;
    }

    private async Task<LayoutValue> GetLayout(DateTime reservationDateTime, string restaurantId, string? layoutId)
    {
        //Получение схемы зала
        LayoutQueryRequest layoutQueryRequest = new();
        layoutQueryRequest.RestaurantId = restaurantId;
        layoutQueryRequest.IsDeleted = false;
        if (layoutId != null)
            layoutQueryRequest.LayoutId = layoutId;

        var layoutResult = await _layoutApiClient.GetLayoutsAsync(layoutQueryRequest);

        if (layoutResult == null)
            throw new Exception("Не удалось получить схему зала для заданного ресторана/ид схемы зала");
        if (layoutResult.State != GrpcContract.ReplyState.Ok || !layoutResult.Layouts.Any())
            throw new Exception("Не удалось получить схему зала\n" + string.Join("\n", layoutResult.Messages));

        var layoutGrpc = layoutResult.Layouts.FirstOrDefault();
        var layoutValue = layoutGrpc.ToDto();
        return layoutValue;
    }
}




