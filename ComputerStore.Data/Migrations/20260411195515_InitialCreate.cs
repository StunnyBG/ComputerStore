using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ComputerStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "TEXT", maxLength: 60, nullable: true),
                    Website = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PcParts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Stock = table.Column<int>(type: "INTEGER", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    ManufacturerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PcParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PcParts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PcParts_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    PcPartId = table.Column<int>(type: "INTEGER", nullable: false)
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
                        name: "FK_OrderItems_PcParts_PcPartId",
                        column: x => x.PcPartId,
                        principalTable: "PcParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Central Processing Units", "CPU" },
                    { 2, "Graphics Processing Units", "GPU" },
                    { 3, "Memory modules", "RAM" },
                    { 4, "SSDs and HDDs", "Storage" },
                    { 5, "Mainboards", "Motherboard" },
                    { 6, "PSUs", "Power Supply" },
                    { 7, "PC Cases", "Case" },
                    { 8, "CPU and case coolers", "Cooling" }
                });

            migrationBuilder.InsertData(
                table: "Manufacturers",
                columns: new[] { "Id", "Country", "Name", "Website" },
                values: new object[,]
                {
                    { 1, "USA", "Intel", "https://www.intel.com" },
                    { 2, "USA", "AMD", "https://www.amd.com" },
                    { 3, "USA", "NVIDIA", "https://www.nvidia.com" },
                    { 4, "USA", "Corsair", "https://www.corsair.com" },
                    { 5, "Taiwan", "G.Skill", "https://www.gskill.com" },
                    { 6, "South Korea", "Samsung", "https://www.samsung.com" },
                    { 7, "USA", "WD", "https://www.westerndigital.com" },
                    { 8, "USA", "Seagate", "https://www.seagate.com" },
                    { 9, "Taiwan", "ASUS", "https://www.asus.com" },
                    { 10, "Taiwan", "MSI", "https://www.msi.com" },
                    { 11, "Germany", "be quiet!", "https://www.bequiet.com" },
                    { 12, "Sweden", "Fractal Design", "https://www.fractal-design.com" },
                    { 13, "USA", "NZXT", "https://www.nzxt.com" },
                    { 14, "Austria", "Noctua", "https://www.noctua.at" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "PasswordHash", "Role", "Username" },
                values: new object[] { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@computerstore.bg", "YP50QG5/NT7ZefNQ8vu2ouhpCl+n0bDDKYPR2LP5X2c=", "Admin", "admin" });

            migrationBuilder.InsertData(
                table: "PcParts",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "ImagePath", "ManufacturerId", "Name", "Price", "Stock" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Intel Core i9-14900K", 589.99m, 15 },
                    { 2, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1, "Intel Core i5-14600K", 299.99m, 30 },
                    { 3, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, "AMD Ryzen 9 7950X", 549.99m, 12 },
                    { 4, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, "AMD Ryzen 5 7600X", 229.99m, 40 },
                    { 5, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, "NVIDIA GeForce RTX 4090", 1599.99m, 8 },
                    { 6, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3, "NVIDIA GeForce RTX 4070", 599.99m, 20 },
                    { 7, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, "AMD Radeon RX 7900 XTX", 999.99m, 10 },
                    { 8, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 2, "AMD Radeon RX 7600", 269.99m, 25 },
                    { 9, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, "Corsair Vengeance 32GB DDR5-6000", 139.99m, 50 },
                    { 10, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 5, "G.Skill Trident Z5 16GB DDR5-5600", 79.99m, 60 },
                    { 11, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 6, "Samsung 990 Pro 1TB NVMe", 109.99m, 45 },
                    { 12, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 7, "WD Black SN850X 2TB NVMe", 179.99m, 30 },
                    { 13, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 8, "Seagate Barracuda 4TB HDD", 74.99m, 35 },
                    { 14, 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 9, "ASUS ROG Maximus Z790 Hero", 599.99m, 10 },
                    { 15, 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 10, "MSI MAG B650 TOMAHAWK WIFI", 229.99m, 22 },
                    { 16, 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, "Corsair RM1000x 1000W 80+ Gold", 189.99m, 20 },
                    { 17, 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 11, "be quiet! Straight Power 850W", 149.99m, 25 },
                    { 18, 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 12, "Fractal Design Torrent", 179.99m, 15 },
                    { 19, 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 13, "NZXT H510 Flow", 99.99m, 20 },
                    { 20, 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 14, "Noctua NH-D15", 99.99m, 30 },
                    { 21, 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4, "Corsair H150i Elite LCD", 259.99m, 18 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturers_Name",
                table: "Manufacturers",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_PcPartId",
                table: "OrderItems",
                column: "PcPartId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PcParts_CategoryId",
                table: "PcParts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PcParts_ManufacturerId",
                table: "PcParts",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "PcParts");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Manufacturers");
        }
    }
}
