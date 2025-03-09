using TavernHelios.MenuService.Common.Entities;
using TavernHelios.MenuService.Common.DTOValues.Menu;
using TavernHelios.MenuService.APICore.Entities.Menu;
using TavernHelios.MenuService.APICore.DTOValues.Menu;
using Google.Protobuf.WellKnownTypes;
using System.Linq.Expressions;
using TavernHelios.Common.Extensions;

namespace TavernHelios.MenuService.Common.Extensions
{
    public static class MenuScheduleExtensions
    {
        public static MenuScheduleReplyValue ToDto(this GrpcContract.MenuService.MenuScheduleFull grpc )
        {
            var result = new MenuScheduleReplyValue();
            result.Id = grpc.Id;
            result.Menu = grpc.Menu.ToDto();
            result.DateTime = grpc.DateTime.ToDateTime().ToUniversalTime();
            result.IsDeleted = grpc.IsDeleted;
            return result;
        }

        public static MenuScheduleEntity ToEntity(this GrpcContract.MenuService.MenuScheduleRequest grpc)
        {
            var result = new MenuScheduleEntity();
            result.Id = grpc.Id;
            result.MenuId = grpc.MenuId;
            result.DateTime = grpc.DateTime.ToDateTime().ToUniversalTime();
            return result;
        }

        public static GrpcContract.MenuService.MenuScheduleRequest ToGrpc(this MenuScheduleUpdateValue value)
        {
            var result = new GrpcContract.MenuService.MenuScheduleRequest();
            result.Id = value.Id;
            result.MenuId = value.MenuId;
            result.DateTime = value.DateTime.ToUniversalTime().ToTimestamp();
            return result;
        }

        public static GrpcContract.MenuService.MenuScheduleFull ToGrpc(this MenuScheduleEntity entity, MenuEntity? menuEntity)
        {
            var result = new GrpcContract.MenuService.MenuScheduleFull();
            result.Id = entity.Id;
            result.Menu = menuEntity?.ToGrpc() ?? throw new ArgumentNullException($@"Не найдено меню [{entity.MenuId}] для расписания [{entity.Id}]");
            result.DateTime = entity.DateTime.ToUniversalTime().ToTimestamp();
            result.IsDeleted = entity.IsDeleted;
            return result;
        }

        public static GrpcContract.MenuService.MenuScheduleRequest ToGrpc(this MenuScheduleCreateValue value)
        {
            var result = new GrpcContract.MenuService.MenuScheduleRequest();
            result.Id = "";
            result.MenuId = value.MenuId;
            result.DateTime = value.DateTime.ToUniversalTime().ToTimestamp();
            return result;
        }

        public static GrpcContract.MenuService.MenuScheduleQueryRequest ToGrpc(this MenuScheduleQueryRequestValue value)
        {
            var result = new GrpcContract.MenuService.MenuScheduleQueryRequest();

            if (value.ScheduleId != null) result.ScheduleId = value.ScheduleId;
            if (value.MenuId != null) result.MenuId = value.MenuId;
            if (value.BeginDate != null) result.BeginDate = value.BeginDate.Value.ToUniversalTime().ToTimestamp();
            if (value.EndDate != null) result.EndDate = value.EndDate.Value.ToUniversalTime().ToTimestamp();
            if (value.IsDeleted != null) result.IsDeleted = value.IsDeleted.Value;

            return result;
        }

        public static Expression<Func<MenuScheduleEntity, bool>> ToQuery(
            this GrpcContract.MenuService.MenuScheduleQueryRequest value
            )
        {
            Expression<Func<MenuScheduleEntity, bool>> result = x => true;

            if (value.HasScheduleId)
            {
                result = result.AndAlso(x => x.Id == value.ScheduleId);
            }

            if (value.HasMenuId)
            {
                result = result.AndAlso(x => x.MenuId == value.MenuId);
            }

            if (value.HasIsDeleted)
            {
                result = result.AndAlso(x => x.IsDeleted == value.IsDeleted);
            }

            if (value.BeginDate != null)
            {
                result = result.AndAlso(x => x.DateTime >= value.BeginDate.ToDateTime().ToUniversalTime());
            }

            if (value.EndDate != null)
            {
                result = result.AndAlso(x => x.DateTime <= value.EndDate.ToDateTime().ToUniversalTime());
            }

            return result;
        }

    }
}
