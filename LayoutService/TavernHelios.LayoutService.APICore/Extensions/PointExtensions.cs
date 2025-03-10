using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TavernHelios.LayoutService.APICore.DTOValues.Layout;
using TavernHelios.LayoutService.APICore.Entities.Layout;

namespace TavernHelios.LayoutService.APICore.Extensions
{
    public static class PointExtensions
    {
        public static PointValue ToDto(this GrpcContract.LayoutService.Point entity)
        {
            var result = new PointValue(entity.X, entity.Y);
            return result;
        }

        public static PointEntity ToEntity(this GrpcContract.LayoutService.Point pointValue)
        {
            var result = new PointEntity(pointValue.X, pointValue.Y);
            return result;
        }

        public static GrpcContract.LayoutService.Point ToGrpc(this PointValue PointValue)
        {
            var result = new GrpcContract.LayoutService.Point();
            result.X = PointValue.X;
            result.Y = PointValue.Y;
            return result;
        }

        public static GrpcContract.LayoutService.Point ToGrpc(this PointEntity PointValue)
        {
            var result = new GrpcContract.LayoutService.Point();
            result.X = PointValue.X;
            result.X = PointValue.X;
            return result;
        }
    }
}
