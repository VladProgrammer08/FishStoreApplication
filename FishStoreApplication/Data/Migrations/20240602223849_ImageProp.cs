using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FishStoreApplication.Data.Migrations
{
    /// <inheritdoc />
    public partial class ImageProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FishImageURL",
                table: "Fishes",
                newName: "SecondaryImageTwo");

            migrationBuilder.AddColumn<string>(
                name: "MainImageURL",
                table: "Fishes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryImageFour",
                table: "Fishes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryImageOne",
                table: "Fishes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryImageThree",
                table: "Fishes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainImageURL",
                table: "Fishes");

            migrationBuilder.DropColumn(
                name: "SecondaryImageFour",
                table: "Fishes");

            migrationBuilder.DropColumn(
                name: "SecondaryImageOne",
                table: "Fishes");

            migrationBuilder.DropColumn(
                name: "SecondaryImageThree",
                table: "Fishes");

            migrationBuilder.RenameColumn(
                name: "SecondaryImageTwo",
                table: "Fishes",
                newName: "FishImageURL");
        }
    }
}
