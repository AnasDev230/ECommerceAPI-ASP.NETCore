using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerceAPI_ASP.NETCore.Migrations
{
    /// <inheritdoc />
    public partial class addOrdersAndOrderItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "430a9028-809a-460e-9e4d-a86b7e628407");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c7c74801-7e0b-419a-a441-bfe8a699425d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e7233f4e-8897-499a-9f5f-fbaf10e35dcf");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("0341ae87-5274-4f07-9628-39cca647a7b3"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("2328e95a-03d3-477a-bb4e-1e3921ce0485"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("3ed96716-87ef-4e70-83f7-6e73c5dab736"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("a4cb508e-732a-482e-a3dc-bc8447ae6ff4"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("69d2e0fb-a8bf-42e2-aead-8225c5a5ddca"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("cb315a7a-acc4-4a23-956b-dec7db6f1467"));

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StockId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_StockId",
                table: "OrderItems",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "430a9028-809a-460e-9e4d-a86b7e628407", "430a9028-809a-460e-9e4d-a86b7e628407", "Vendor", "VENDOR" },
                    { "c7c74801-7e0b-419a-a441-bfe8a699425d", "c7c74801-7e0b-419a-a441-bfe8a699425d", "Customer", "CUSTOMER" },
                    { "e7233f4e-8897-499a-9f5f-fbaf10e35dcf", "e7233f4e-8897-499a-9f5f-fbaf10e35dcf", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "ParentCategoryId", "UrlHandle" },
                values: new object[,]
                {
                    { new Guid("69d2e0fb-a8bf-42e2-aead-8225c5a5ddca"), "Fashion", null, "fashion" },
                    { new Guid("cb315a7a-acc4-4a23-956b-dec7db6f1467"), "Electronics", null, "electronics" },
                    { new Guid("0341ae87-5274-4f07-9628-39cca647a7b3"), "Men's Clothing", new Guid("69d2e0fb-a8bf-42e2-aead-8225c5a5ddca"), "mens-clothing" },
                    { new Guid("2328e95a-03d3-477a-bb4e-1e3921ce0485"), "Laptops", new Guid("cb315a7a-acc4-4a23-956b-dec7db6f1467"), "laptops" },
                    { new Guid("3ed96716-87ef-4e70-83f7-6e73c5dab736"), "Mobiles", new Guid("cb315a7a-acc4-4a23-956b-dec7db6f1467"), "mobiles" },
                    { new Guid("a4cb508e-732a-482e-a3dc-bc8447ae6ff4"), "Women's Clothing", new Guid("69d2e0fb-a8bf-42e2-aead-8225c5a5ddca"), "womens-clothing" }
                });
        }
    }
}
