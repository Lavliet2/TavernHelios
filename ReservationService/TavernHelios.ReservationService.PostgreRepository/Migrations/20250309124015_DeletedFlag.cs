using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavernHelios.ReservationService.PostgreRepository.Migrations
{
    /// <inheritdoc />
    public partial class DeletedFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ReservationEntity",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ReservationEntity");
        }
    }
}
