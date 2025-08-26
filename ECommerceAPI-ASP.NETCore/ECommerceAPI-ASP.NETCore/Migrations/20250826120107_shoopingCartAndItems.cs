using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerceAPI_ASP.NETCore.Migrations
{
    /// <inheritdoc />
    public partial class shoopingCartAndItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("09404ec7-53f4-4f29-a22c-8e67f6477bf9"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("146f655c-6d70-4cd5-b7fa-c7f3182284ff"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("5d1c6bd6-b906-4e49-9e44-62bd5fff4224"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("f1147739-57b7-4973-b477-1db26f18e9fd"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("18556e31-9b9e-413e-9571-8575eca55132"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("5deb0062-9617-4fe1-a4d5-f0e81bae3546"));

            migrationBuilder.CreateTable(
                name: "ShoppingCarts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShoppingCartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StockId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCartItems_ShoppingCarts_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingCartItems_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "ParentCategoryId", "UrlHandle" },
                values: new object[,]
                {
                    { new Guid("ae73d929-14d7-4f7e-ab13-a8acf636318f"), "Electronics", null, "electronics" },
                    { new Guid("cc336026-ff97-465c-80f0-21006d33ed85"), "Fashion", null, "fashion" },
                    { new Guid("26840ebd-60e0-43d0-9ac8-3c310264c6fa"), "Laptops", new Guid("ae73d929-14d7-4f7e-ab13-a8acf636318f"), "laptops" },
                    { new Guid("2aedd4ad-7d67-412e-8ee4-80e36caaa709"), "Women's Clothing", new Guid("cc336026-ff97-465c-80f0-21006d33ed85"), "womens-clothing" },
                    { new Guid("50993a1b-05fe-4a3b-bb19-cdb54671057e"), "Men's Clothing", new Guid("cc336026-ff97-465c-80f0-21006d33ed85"), "mens-clothing" },
                    { new Guid("71861c96-6da2-43ec-8281-fd71055ac52f"), "Mobiles", new Guid("ae73d929-14d7-4f7e-ab13-a8acf636318f"), "mobiles" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItems_ShoppingCartId",
                table: "ShoppingCartItems",
                column: "ShoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItems_StockId",
                table: "ShoppingCartItems",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_CustomerId",
                table: "ShoppingCarts",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingCartItems");

            migrationBuilder.DropTable(
                name: "ShoppingCarts");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("26840ebd-60e0-43d0-9ac8-3c310264c6fa"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("2aedd4ad-7d67-412e-8ee4-80e36caaa709"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("50993a1b-05fe-4a3b-bb19-cdb54671057e"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("71861c96-6da2-43ec-8281-fd71055ac52f"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("ae73d929-14d7-4f7e-ab13-a8acf636318f"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("cc336026-ff97-465c-80f0-21006d33ed85"));

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "ParentCategoryId", "UrlHandle" },
                values: new object[,]
                {
                    { new Guid("18556e31-9b9e-413e-9571-8575eca55132"), "Electronics", null, "electronics" },
                    { new Guid("5deb0062-9617-4fe1-a4d5-f0e81bae3546"), "Fashion", null, "fashion" },
                    { new Guid("09404ec7-53f4-4f29-a22c-8e67f6477bf9"), "Men's Clothing", new Guid("5deb0062-9617-4fe1-a4d5-f0e81bae3546"), "mens-clothing" },
                    { new Guid("146f655c-6d70-4cd5-b7fa-c7f3182284ff"), "Laptops", new Guid("18556e31-9b9e-413e-9571-8575eca55132"), "laptops" },
                    { new Guid("5d1c6bd6-b906-4e49-9e44-62bd5fff4224"), "Mobiles", new Guid("18556e31-9b9e-413e-9571-8575eca55132"), "mobiles" },
                    { new Guid("f1147739-57b7-4973-b477-1db26f18e9fd"), "Women's Clothing", new Guid("5deb0062-9617-4fe1-a4d5-f0e81bae3546"), "womens-clothing" }
                });
        }
    }
}
