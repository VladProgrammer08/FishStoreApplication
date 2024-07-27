using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FishStoreApplication.Data.Migrations
{
    /// <inheritdoc />
    public partial class Aquarium : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aquariums",
                columns: table => new
                {
                    AquariumId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AquariumGallons = table.Column<double>(type: "float", nullable: false),
                    AquariumPrice = table.Column<double>(type: "float", nullable: false),
                    AquariumBrand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AquariumMaterial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondaryImageOne = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondaryImageTwo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondaryImageThree = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondaryImageFour = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AquariumWarranty = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aquariums", x => x.AquariumId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aquariums");
        }
    }
}
