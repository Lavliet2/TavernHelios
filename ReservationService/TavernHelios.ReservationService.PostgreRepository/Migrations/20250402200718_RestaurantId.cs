using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavernHelios.ReservationService.PostgreRepository.Migrations
{
    /// <inheritdoc />
    public partial class RestaurantId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RestaurantId",
                table: "ReservationEntity",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "ReservationEntity",
                keyColumn: "Id",
                keyValue: null,
                column: "RestaurantId",
                value: "TestRestaurantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "ReservationEntity");
        }
    }
}
