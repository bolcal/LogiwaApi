using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogiwaApi.Migrations
{
    public partial class seeds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinStockToLive = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    StockQuantity = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "MinStockToLive", "Name" },
                values: new object[,]
                {
                    { 1, 10, "Headphones" },
                    { 2, 50, "Mouse" },
                    { 3, 10, "Keyboard" },
                    { 4, 10, "Monitor" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "StockQuantity", "Title" },
                values: new object[] { 3, null, "Wired Mouse Optical Tracking", 100, "Logitech M150" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "StockQuantity", "Title" },
                values: new object[,]
                {
                    { 1, 1, "Bluetooth headphones with noise cancelling In-Ear", 20, "Sony WF-1000" },
                    { 2, 1, "Over-the-Ear Headphones", 40, "Phillips T90" },
                    { 4, 3, null, 4, "Logitech Internet Pro" },
                    { 5, 3, null, 8, "Asus Rog" },
                    { 6, 4, "Ultrawide monitor 75hz Full HD", 50, "LG 29WL500" },
                    { 7, 2, "Wired Gaming Mouse Optical Tracking 8000 DPI ", 30, "Logitech G502" },
                    { 8, 2, "Optical Wireless Gaming Mouse", 55, "Msi GG GM08" },
                    { 9, 4, "IPS 165Hz HDR Gaming Monitor", 25, "HP X34" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
