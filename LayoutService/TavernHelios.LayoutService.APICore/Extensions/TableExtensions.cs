using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TavernHelios.LayoutService.APICore.DTOValues.Layout;
using TavernHelios.LayoutService.APICore.Entities.Layout;

namespace TavernHelios.LayoutService.APICore.Extensions
{
    public static class TableExtensions
    {
        public static TableValue ToDto(this GrpcContract.LayoutService.Table entity)
        {
            var result = new TableValue();
            result.Description = entity.Description;
            result.Name = entity.Name;
            result.P1 = entity.P1.ToDto();
            result.P2 = entity.P2.ToDto();
            result.P3 = entity.P3.ToDto();
            result.P4 = entity.P4.ToDto();
            result.Seats = new List<SeatValue>(entity.Seats.Select(x => x.ToDto()));
            return result;
        }

        public static TableEntity ToEntity(this GrpcContract.LayoutService.Table tableValue)
        {
            var result = new TableEntity();
            result.Name = tableValue.Name;
            result.Description = tableValue.Description;
            result.P1 = tableValue.P1.ToEntity();
            result.P2 = tableValue.P2.ToEntity();
            result.P3 = tableValue.P3.ToEntity();
            result.P4 = tableValue.P4.ToEntity();
            result.Seats = new List<SeatEntity>(tableValue.Seats.Select(x => x.ToEntity()));
            return result;
        }

        public static GrpcContract.LayoutService.Table ToGrpc(this TableValue tableValue)
        {
            var result = new GrpcContract.LayoutService.Table();
            result.Name = tableValue.Name;
            result.Description = tableValue.Description;
            result.P1 = tableValue.P1.ToGrpc();
            result.P2 = tableValue.P2.ToGrpc();
            result.P3 = tableValue.P3.ToGrpc();
            result.P4 = tableValue.P4.ToGrpc();
            result.Seats.AddRange(tableValue.Seats.Select(x => x.ToGrpc()));
            return result;
        }

        public static GrpcContract.LayoutService.Table ToGrpc(this TableEntity tableValue)
        {
            var result = new GrpcContract.LayoutService.Table();
            result.Name = tableValue.Name;
            result.Description = tableValue.Description;
            result.P1 = tableValue.P1.ToGrpc();
            result.P2 = tableValue.P2.ToGrpc();
            result.P3 = tableValue.P3.ToGrpc();
            result.P4 = tableValue.P4.ToGrpc();
            result.Seats.AddRange(tableValue.Seats.Select(x => x.ToGrpc()));
            return result;
        }
    }
}
