using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavernHelios.ReservationService.PostgreRepository.Migrations
{
    /// <inheritdoc />
    public partial class SeatNumber_TableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SeatNumber",
                table: "ReservationEntity",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TableName",
                table: "ReservationEntity",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeatNumber",
                table: "ReservationEntity");

            migrationBuilder.DropColumn(
                name: "TableName",
                table: "ReservationEntity");
        }
    }
}
