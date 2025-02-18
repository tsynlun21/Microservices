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
                    table.UniqueConstraint("AK_VehicleModels_Model", x => x.Model);
                });

            migrationBuilder.CreateTable(
                name: "Receipts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShowroomKey = table.Column<long>(type: "bigint", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalItemsCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receipts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Receipts_Showrooms_ShowroomKey",
                        column: x => x.ShowroomKey,
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
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    ShowroomEntityId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtraItems", x => x.Id);
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
                    Vin = table.Column<string>(type: "text", nullable: false),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Mileage = table.Column<decimal>(type: "numeric", nullable: false),
                    VehicleModelId = table.Column<long>(type: "bigint", nullable: false),
                    Color = table.Column<int>(type: "integer", nullable: false),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "SoldExtraItemsEntity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    VehicleModelKey = table.Column<string>(type: "text", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    ReceiptEntityId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoldExtraItemsEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SoldExtraItemsEntity_Receipts_ReceiptEntityId",
                        column: x => x.ReceiptEntityId,
                        principalTable: "Receipts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SoldExtraItemsEntity_VehicleModels_VehicleModelKey",
                        column: x => x.VehicleModelKey,
                        principalTable: "VehicleModels",
                        principalColumn: "Model",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SoldVehicleEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReceiptKey = table.Column<long>(type: "bigint", nullable: false),
                    Vin = table.Column<string>(type: "text", nullable: false),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Mileage = table.Column<decimal>(type: "numeric", nullable: false),
                    Color = table.Column<int>(type: "integer", nullable: false),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoldVehicleEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SoldVehicleEntity_Receipts_ReceiptKey",
                        column: x => x.ReceiptKey,
                        principalTable: "Receipts",
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
                columns: new[] { "Id", "Count", "Name", "Price", "ShowroomEntityId", "Type", "VehicleModelEntityKey" },
                values: new object[,]
                {
                    { 1L, 10, "Harman Kardon audio system", 185000m, null, 1, 1L },
                    { 2L, 10, "Lip spoiler", 45000m, null, 2, 1L },
                    { 3L, 10, "LED lights", 310000m, null, 1, 1L },
                    { 4L, 10, "Power Wilkins Audio system", 160000m, null, 1, 2L },
                    { 5L, 3, "19R Alpine wheels set", 750000m, null, 2, 2L },
                    { 6L, 3, "AMG kit", 300000m, null, 2, 3L },
                    { 7L, 3, "Ceramic breaks system", 915000m, null, 2, 4L }
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "Brand", "Color", "Mileage", "Price", "ReleaseDate", "ShowroomEntityKey", "VehicleModelId", "Vin" },
                values: new object[,]
                {
                    { 1L, "BMW", 4, 120000m, 2350000m, new DateOnly(2015, 1, 1), 1L, 1L, "23D34EC8-F83F-4BED-95D6-05BDE4F76222" },
                    { 2L, "BMW", 3, 85000m, 3200000m, new DateOnly(2017, 6, 13), 1L, 1L, "6786D736-1C1D-4375-8A35-6594E5133823" },
                    { 3L, "BMW", 3, 20000m, 10200000m, new DateOnly(2020, 11, 1), 1L, 2L, "8E10D3F9-FE4A-4023-9E44-505249AE95C5" },
                    { 4L, "Mercedes-Benz", 1, 89000m, 3200000m, new DateOnly(2016, 5, 10), 2L, 3L, "80070953-A133-41C2-AD49-BD51C30FC7AE" },
                    { 5L, "Mercedes-Benz", 5, 52000m, 9450000m, new DateOnly(2018, 3, 12), 2L, 4L, "BCC89D79-35D9-4AC2-8597-A837DED42699" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExtraItems_Name_VehicleModelEntityKey",
                table: "ExtraItems",
                columns: new[] { "Name", "VehicleModelEntityKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExtraItems_Name_VehicleModelEntityKey_Type",
                table: "ExtraItems",
                columns: new[] { "Name", "VehicleModelEntityKey", "Type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExtraItems_ShowroomEntityId",
                table: "ExtraItems",
                column: "ShowroomEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtraItems_VehicleModelEntityKey",
                table: "ExtraItems",
                column: "VehicleModelEntityKey");

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_ShowroomKey",
                table: "Receipts",
                column: "ShowroomKey");

            migrationBuilder.CreateIndex(
                name: "IX_SoldExtraItemsEntity_ReceiptEntityId",
                table: "SoldExtraItemsEntity",
                column: "ReceiptEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_SoldExtraItemsEntity_VehicleModelKey",
                table: "SoldExtraItemsEntity",
                column: "VehicleModelKey");

            migrationBuilder.CreateIndex(
                name: "IX_SoldVehicleEntity_ReceiptKey",
                table: "SoldVehicleEntity",
                column: "ReceiptKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleModels_Model",
                table: "VehicleModels",
                column: "Model",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_Brand_VehicleModelId_Color_Vin",
                table: "Vehicles",
                columns: new[] { "Brand", "VehicleModelId", "Color", "Vin" },
                unique: true);

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
                name: "SoldExtraItemsEntity");

            migrationBuilder.DropTable(
                name: "SoldVehicleEntity");

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
