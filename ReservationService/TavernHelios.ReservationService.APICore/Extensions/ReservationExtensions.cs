

using Google.Protobuf.WellKnownTypes;
using TavernHelios.ReservationService.APICore.DTOValues;
using TavernHelios.ReservationService.APICore.Entities;

namespace TavernHelios.ReservationService.ApiCore.Extensions
{
    public static class ReervationExtensions
    {
        public static ReservationValue ToDto(this GrpcContract.ReservationService.Reservation entity)
        {
            var result = new ReservationValue();
            result.Id = entity.Id;
            result.Date = entity.Date.ToDateTime();
            result.PersonId = entity.PersonId;
            result.DishIds = entity.DishIds.ToList();
            return result;
        }

        public static ReservationEntity ToEntity(this GrpcContract.ReservationService.Reservation value)
        {
            var result = new ReservationEntity();
            result.Id = long.Parse(value.Id);
            result.PersonId = value.PersonId;
            result.Date = value.Date.ToDateTime();
            result.DishIds = value.DishIds.ToList();
            return result;
        }

        public static GrpcContract.ReservationService.Reservation ToGrpc(this ReservationValue value)
        {
            var result = new GrpcContract.ReservationService.Reservation();
            result.Id = value.Id;
            result.PersonId = value.PersonId;
            result.Date = value.Date.ToTimestamp();
            result.DishIds.AddRange(value.DishIds);
            return result;
        }

        public static GrpcContract.ReservationService.Reservation ToGrpc(this ReservationEntity value)
        {
            var result = new GrpcContract.ReservationService.Reservation();
            result.Id = value.Id.ToString();
            result.PersonId = value.PersonId;
            result.Date = value.Date.ToTimestamp();
            result.DishIds.AddRange(value.DishIds);
            return result;
        }

    }
}
