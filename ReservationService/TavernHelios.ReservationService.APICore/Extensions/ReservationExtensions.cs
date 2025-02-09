

using System.Linq.Expressions;
using Google.Protobuf.WellKnownTypes;
using TavernHelios.ReservationService.APICore.DTOValues;
using TavernHelios.ReservationService.APICore.Entities;
using TavernHelios.ReservationService.APICore.Extensions;

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
            foreach(var dishId in value.DishIds)
            {
                result.DishReservations.Add(new DishReservationEntity() { DishId = dishId });
            }
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

        public static GrpcContract.ReservationService.Reservation ToGrpc(this ReservationCreateValue value)
        {
            var result = new GrpcContract.ReservationService.Reservation();
            result.Id = "0";
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
            result.DishIds.AddRange(value.DishReservations.Select(x => x.DishId));
            return result;
        }

        public static GrpcContract.ReservationService.ReservationQueryRequest ToGrpc(this ReservationQueryRequestValue value)
        {
            var result = new GrpcContract.ReservationService.ReservationQueryRequest();

            if (value.ReservationId != null) result.ReservationId = value.ReservationId.Value;
            if (value.PersonId != null) result.PersonId = value.PersonId;
            if (value.DishId != null) result.DishId = value.DishId;
            if (value.BeginDate != null) result.BeginDate = value.BeginDate.Value.ToTimestamp();
            if (value.EndDate != null) result.EndDate = value.EndDate.Value.ToTimestamp();

            return result;
        }

        public static Expression<Func<ReservationEntity, bool>> ToQuery(
            this GrpcContract.ReservationService.ReservationQueryRequest value
            )
        {
            Expression<Func<ReservationEntity, bool>> result = x => true;
            
            if (value.HasReservationId) 
            {
                result = result.AndAlso(x => x.Id == value.ReservationId);
            }

            if (value.HasPersonId)
            {
                result = result.AndAlso(x => x.PersonId == value.PersonId);
            }

            if (value.HasDishId)
            {
                result = result.AndAlso(x => x.DishReservations.FirstOrDefault(d => d.DishId == value.DishId) != null);
            }

            if (value.BeginDate != null)
            {
                result = result.AndAlso(x => x.Date >= value.BeginDate.ToDateTime());
            }

            if (value.EndDate != null)
            {
                result = result.AndAlso(x => x.Date <= value.EndDate.ToDateTime());
            }

            return result;
        }

    }
}
