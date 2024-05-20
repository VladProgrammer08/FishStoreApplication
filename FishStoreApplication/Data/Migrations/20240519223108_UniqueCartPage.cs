using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FishStoreApplication.Data.Migrations
{
    /// <inheritdoc />
    public partial class UniqueCartPage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CartFishViewModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CartFishViewModel");
        }
    }
}
