using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductScraper.DataAccess.Migrations
{
    public partial class changeproductupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastSyncTime",
                table: "Products",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "NewValue",
                table: "ProductChanges",
                type: "character varying(1450)",
                maxLength: 1450,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewValue",
                table: "ProductChanges");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastSyncTime",
                table: "Products",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }
    }
}
