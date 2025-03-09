﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TavernHelios.LayoutService.APICore.DTOValues.Layout;
using TavernHelios.LayoutService.APICore.Entities.Layout;

namespace TavernHelios.LayoutService.APICore.Extensions
{
    public static class SeatExtensions
    {
        public static SeatValue ToDto(this GrpcContract.LayoutService.Seat entity)
        {
            var result = new SeatValue();
            result.Number = entity.Number;
            result.Desctiption = entity.Description;
            result.Center = entity.Center.ToDto();
            result.Alignment = entity.Alignment.ToDto();
            return result;
        }

        public static SeatEntity ToEntity(this GrpcContract.LayoutService.Seat seatValue)
        {
            var result = new SeatEntity();
            result.Number = seatValue.Number;
            result.Desctiption = seatValue.Description;
            result.Alignment = seatValue.Alignment.ToEntity();
            result.Center = seatValue.Center.ToEntity();
            return result;
        }

        public static GrpcContract.LayoutService.Seat ToGrpc(this SeatValue seatValue)
        {
            var result = new GrpcContract.LayoutService.Seat();
            result.Number = seatValue.Number;
            result.Description = seatValue.Desctiption;
            result.Alignment = seatValue.Alignment.ToGrpc();
            result.Center = seatValue.Center.ToGrpc();
            
            return result;
        }

        public static GrpcContract.LayoutService.Seat ToGrpc(this SeatEntity seatValue)
        {
            var result = new GrpcContract.LayoutService.Seat();
            result.Number = seatValue.Number;
            result.Description = seatValue.Desctiption;
            result.Center = seatValue.Center.ToGrpc();
            result.Alignment = seatValue.Center.ToGrpc();
            return result;
        }
    }
}
