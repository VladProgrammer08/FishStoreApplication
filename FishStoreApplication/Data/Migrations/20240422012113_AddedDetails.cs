using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FishStoreApplication.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Fishes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "FishCareLevel",
                table: "Fishes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FishEnvironment",
                table: "Fishes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FishImageURL",
                table: "Fishes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FishSize",
                table: "Fishes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FishSpecialDiet",
                table: "Fishes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FishTemperament",
                table: "Fishes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FishWarranty",
                table: "Fishes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FishCareLevel",
                table: "Fishes");

            migrationBuilder.DropColumn(
                name: "FishEnvironment",
                table: "Fishes");

            migrationBuilder.DropColumn(
                name: "FishImageURL",
                table: "Fishes");

            migrationBuilder.DropColumn(
                name: "FishSize",
                table: "Fishes");

            migrationBuilder.DropColumn(
                name: "FishSpecialDiet",
                table: "Fishes");

            migrationBuilder.DropColumn(
                name: "FishTemperament",
                table: "Fishes");

            migrationBuilder.DropColumn(
                name: "FishWarranty",
                table: "Fishes");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Fishes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
