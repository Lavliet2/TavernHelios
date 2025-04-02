using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavernHelios.ReservationService.PostgreRepository.Migrations
{
    /// <inheritdoc />
    public partial class RestaurantId_to_LayoutId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RestaurantId",
                table: "ReservationEntity",
                newName: "LayoutId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LayoutId",
                table: "ReservationEntity",
                newName: "RestaurantId");
        }
    }
}
