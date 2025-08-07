using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Sets_API.Migrations
{
    /// <inheritdoc />
    public partial class SETS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Storage",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ImageSource = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storage", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Boxes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StorageID = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    QRcode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boxes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Boxes_Storage_StorageID",
                        column: x => x.StorageID,
                        principalTable: "Storage",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Sets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    setID = table.Column<int>(type: "integer", nullable: true)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    boxID = table.Column<int>(type: "integer", nullable: true),
                    number = table.Column<string>(type: "text", nullable: false),
                    numberVariant = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false),
                    theme = table.Column<string>(type: "text", nullable: true),
                    themeGroup = table.Column<string>(type: "text", nullable: true),
                    subTheme = table.Column<string>(type: "text", nullable: true),
                    category = table.Column<string>(type: "text", nullable: true),
                    released = table.Column<bool>(type: "boolean", nullable: true),
                    pieces = table.Column<int>(type: "integer", nullable: true),
                    minifigs = table.Column<int>(type: "integer", nullable: true),
                    packagingType = table.Column<string>(type: "text", nullable: true),
                    availability = table.Column<string>(type: "text", nullable: true),
                    instructionsCount = table.Column<int>(type: "integer", nullable: true),
                    LEGOCom_US_RetailPrice = table.Column<double>(type: "double precision", nullable: true),
                    LEGOCom_US_DateFirstAvailable = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LEGOCom_US_DateLastAvailable = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LEGOCom_UK_RetailPrice = table.Column<double>(type: "double precision", nullable: true),
                    LEGOCom_UK_DateFirstAvailable = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LEGOCom_UK_DateLastAvailable = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LEGOCom_CA_RetailPrice = table.Column<double>(type: "double precision", nullable: true),
                    LEGOCom_CA_DateFirstAvailable = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LEGOCom_CA_DateLastAvailable = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LEGOCom_DE_RetailPrice = table.Column<double>(type: "double precision", nullable: true),
                    LEGOCom_DE_DateFirstAvailable = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LEGOCom_DE_DateLastAvailable = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Image_ThumbnailURL = table.Column<string>(type: "text", nullable: true),
                    Image_ImageURL = table.Column<string>(type: "text", nullable: true),
                    AgeRange_Id = table.Column<int>(type: "integer", nullable: true),
                    AgeRange_Min = table.Column<int>(type: "integer", nullable: true),
                    AgeRange_Max = table.Column<int>(type: "integer", nullable: true),
                    Dimensions_Height = table.Column<double>(type: "double precision", nullable: true),
                    Dimensions_Width = table.Column<double>(type: "double precision", nullable: true),
                    Dimensions_Depth = table.Column<double>(type: "double precision", nullable: true),
                    Dimensions_Weight = table.Column<double>(type: "double precision", nullable: true),
                    Barcode_EAN = table.Column<string>(type: "text", nullable: true),
                    Barcode_UPC = table.Column<string>(type: "text", nullable: true),
                    ExtendedData_BrickTags = table.Column<List<string>>(type: "text[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sets_Boxes_boxID",
                        column: x => x.boxID,
                        principalTable: "Boxes",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Boxes_StorageID",
                table: "Boxes",
                column: "StorageID");

            migrationBuilder.CreateIndex(
                name: "IX_Sets_boxID",
                table: "Sets",
                column: "boxID");

            migrationBuilder.CreateIndex(
                name: "IX_Sets_name",
                table: "Sets",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_Sets_number",
                table: "Sets",
                column: "number");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sets");

            migrationBuilder.DropTable(
                name: "Boxes");

            migrationBuilder.DropTable(
                name: "Storage");
        }
    }
}
