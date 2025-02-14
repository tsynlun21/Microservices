using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarsShowroom.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Showrooms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Address = table.Column<string>(type: "text", nullable: false),
                    ContactNumber = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Showrooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleModels",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Model = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Receipts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShowroomEntityKey = table.Column<long>(type: "bigint", nullable: false),
                    ShoowRoomEntityKey = table.Column<int>(type: "integer", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalItems = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receipts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Receipts_Showrooms_ShowroomEntityKey",
                        column: x => x.ShowroomEntityKey,
                        principalTable: "Showrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExtraItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    VehicleModelEntityKey = table.Column<long>(type: "bigint", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    ReceiptEntityId = table.Column<long>(type: "bigint", nullable: true),
                    ShowroomEntityId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtraItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExtraItems_Receipts_ReceiptEntityId",
                        column: x => x.ReceiptEntityId,
                        principalTable: "Receipts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExtraItems_Showrooms_ShowroomEntityId",
                        column: x => x.ShowroomEntityId,
                        principalTable: "Showrooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExtraItems_VehicleModels_VehicleModelEntityKey",
                        column: x => x.VehicleModelEntityKey,
                        principalTable: "VehicleModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShowroomEntityKey = table.Column<long>(type: "bigint", nullable: false),
                    Uid = table.Column<string>(type: "text", nullable: false),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    VehicleModelId = table.Column<long>(type: "bigint", nullable: false),
                    Color = table.Column<int>(type: "integer", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReceiptEntityId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Receipts_ReceiptEntityId",
                        column: x => x.ReceiptEntityId,
                        principalTable: "Receipts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vehicles_Showrooms_ShowroomEntityKey",
                        column: x => x.ShowroomEntityKey,
                        principalTable: "Showrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vehicles_VehicleModels_VehicleModelId",
                        column: x => x.VehicleModelId,
                        principalTable: "VehicleModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Showrooms",
                columns: new[] { "Id", "Address", "ContactNumber" },
                values: new object[,]
                {
                    { 1L, "Ростов-на-Дону, ул. Еременко 28, 'BMW Motors'", "79959303330" },
                    { 2L, "Ростов-на-Дону, ул. Шаболдаева 105, 'Mercedes-Benz club'", "79959303331" }
                });

            migrationBuilder.InsertData(
                table: "VehicleModels",
                columns: new[] { "Id", "Model" },
                values: new object[,]
                {
                    { 1L, "BMW 325" },
                    { 2L, "BMW M5 F90" },
                    { 3L, "Mercedes-Benz E200" },
                    { 4L, "Mercedes-Benz CLS63 AMG" }
                });

            migrationBuilder.InsertData(
                table: "ExtraItems",
                columns: new[] { "Id", "Count", "Name", "ReceiptEntityId", "ShowroomEntityId", "Type", "VehicleModelEntityKey" },
                values: new object[,]
                {
                    { 1L, 10, "Harman Kardon audio system", null, null, 1, 1L },
                    { 2L, 10, "Lip spoiler", null, null, 2, 1L },
                    { 3L, 10, "LED lights", null, null, 1, 1L },
                    { 4L, 10, "Power Wilkins Audio system", null, null, 1, 2L },
                    { 5L, 3, "19R Alpine wheels", null, null, 2, 2L },
                    { 6L, 3, "AMG kit", null, null, 2, 3L },
                    { 7L, 3, "Ceramic breaks system", null, null, 2, 4L }
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "Brand", "Color", "Price", "ReceiptEntityId", "ReleaseDate", "ShowroomEntityKey", "Uid", "VehicleModelId" },
                values: new object[,]
                {
                    { 1L, "BMW", 4, 3000000m, null, new DateTime(2023, 3, 4, 0, 0, 0, 0, DateTimeKind.Utc), 1L, "23D34EC8-F83F-4BED-95D6-05BDE4F76222", 1L },
                    { 2L, "BMW", 3, 3200000m, null, new DateTime(2023, 3, 4, 0, 0, 0, 0, DateTimeKind.Utc), 1L, "6786D736-1C1D-4375-8A35-6594E5133823", 1L },
                    { 3L, "BMW", 3, 10200000m, null, new DateTime(2023, 3, 4, 0, 0, 0, 0, DateTimeKind.Utc), 1L, "8E10D3F9-FE4A-4023-9E44-505249AE95C5", 2L },
                    { 4L, "Mercedes-Benz", 1, 3200000m, null, new DateTime(2023, 3, 4, 0, 0, 0, 0, DateTimeKind.Utc), 2L, "80070953-A133-41C2-AD49-BD51C30FC7AE", 3L },
                    { 5L, "Mercedes-Benz", 5, 9450000m, null, new DateTime(2023, 3, 4, 0, 0, 0, 0, DateTimeKind.Utc), 2L, "BCC89D79-35D9-4AC2-8597-A837DED42699", 4L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExtraItems_ReceiptEntityId",
                table: "ExtraItems",
                column: "ReceiptEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtraItems_ShowroomEntityId",
                table: "ExtraItems",
                column: "ShowroomEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtraItems_VehicleModelEntityKey",
                table: "ExtraItems",
                column: "VehicleModelEntityKey");

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_ShowroomEntityKey",
                table: "Receipts",
                column: "ShowroomEntityKey");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ReceiptEntityId",
                table: "Vehicles",
                column: "ReceiptEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ShowroomEntityKey",
                table: "Vehicles",
                column: "ShowroomEntityKey");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleModelId",
                table: "Vehicles",
                column: "VehicleModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExtraItems");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Receipts");

            migrationBuilder.DropTable(
                name: "VehicleModels");

            migrationBuilder.DropTable(
                name: "Showrooms");
        }
    }
}
