using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductScraper.Migrations
{
    public partial class ProductChangeAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductChanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ASIN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ChangedVar = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ChangeValue = table.Column<string>(type: "nvarchar(1450)", maxLength: 1450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductChanges", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductChanges");
        }
    }
}
