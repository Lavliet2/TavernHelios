using Microsoft.EntityFrameworkCore.Migrations;
using TavernHelios.Auth.Data.Models;

#nullable disable

namespace TavernHelios.Auth.Migrations
{
    /// <inheritdoc />
    public partial class AddManagerRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData("Roles", columns: new[] { nameof(Role.Id), nameof(Role.Name) },
                values: new object[,]
                {
                    { (int)RoleEnum.Manager, "Менеджер" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
