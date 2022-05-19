using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductScarper.DataAccess.Migrations
{
    public partial class ProductTrackingUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FollowingUsers",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "TrackingUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackingUsers_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrackingUsers_ProductId",
                table: "TrackingUsers",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrackingUsers");

            migrationBuilder.AddColumn<int>(
                name: "FollowingUsers",
                table: "Products",
                type: "int",
                nullable: true);
        }
    }
}
