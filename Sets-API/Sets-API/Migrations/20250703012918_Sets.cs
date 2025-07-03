using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Sets_API.Migrations
{
    /// <inheritdoc />
    public partial class Sets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Box",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        Name = table.Column<string>(type: "text", nullable: true),
            //        Description = table.Column<string>(type: "text", nullable: true),
            //        QRcode = table.Column<string>(type: "text", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("Box_pkey", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Sets",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        BoxID = table.Column<int>(type: "integer", nullable: true),
            //        Number = table.Column<string>(type: "text", nullable: false),
            //        NumberVariant = table.Column<int>(type: "integer", nullable: false),
            //        Name = table.Column<string>(type: "text", nullable: false),
            //        Year = table.Column<int>(type: "integer", nullable: false),
            //        Theme = table.Column<string>(type: "text", nullable: true),
            //        ThemeGroup = table.Column<string>(type: "text", nullable: true),
            //        Subtheme = table.Column<string>(type: "text", nullable: true),
            //        Category = table.Column<string>(type: "text", nullable: true),
            //        Released = table.Column<int>(type: "integer", nullable: true),
            //        Pieces = table.Column<int>(type: "integer", nullable: true),
            //        Minifigs = table.Column<int>(type: "integer", nullable: true),
            //        Image_Id = table.Column<int>(type: "integer", nullable: true),
            //        Image_ThumbnailURL = table.Column<string>(type: "text", nullable: true),
            //        Image_ImageURL = table.Column<string>(type: "text", nullable: true),
            //        LEGOCom_Id = table.Column<int>(type: "integer", nullable: true),
            //        LEGOCom_US_RetailPrice = table.Column<string>(type: "text", nullable: true),
            //        LEGOCom_US_DateFirstAvailable = table.Column<string>(type: "text", nullable: true),
            //        LEGOCom_US_DateLastAvailable = table.Column<string>(type: "text", nullable: true),
            //        LEGOCom_UK_RetailPrice = table.Column<string>(type: "text", nullable: true),
            //        LEGOCom_UK_DateFirstAvailable = table.Column<string>(type: "text", nullable: true),
            //        LEGOCom_UK_DateLastAvailable = table.Column<string>(type: "text", nullable: true),
            //        LEGOCom_CA_RetailPrice = table.Column<string>(type: "text", nullable: true),
            //        LEGOCom_CA_DateFirstAvailable = table.Column<string>(type: "text", nullable: true),
            //        LEGOCom_CA_DateLastAvailable = table.Column<string>(type: "text", nullable: true),
            //        LEGOCom_DE_RetailPrice = table.Column<string>(type: "text", nullable: true),
            //        LEGOCom_DE_DateFirstAvailable = table.Column<string>(type: "text", nullable: true),
            //        LEGOCom_DE_DateLastAvailable = table.Column<string>(type: "text", nullable: true),
            //        PackagingType = table.Column<string>(type: "text", nullable: true),
            //        Availability = table.Column<string>(type: "text", nullable: true),
            //        InstructionsCount = table.Column<int>(type: "integer", nullable: true),
            //        AgeRange_Id = table.Column<int>(type: "integer", nullable: true),
            //        AgeRange_Min = table.Column<int>(type: "integer", nullable: true),
            //        AgeRange_Max = table.Column<int>(type: "integer", nullable: true),
            //        Dimensions_Height = table.Column<float>(type: "real", nullable: true),
            //        Dimensions_Width = table.Column<float>(type: "real", nullable: true),
            //        Dimensions_Depth = table.Column<float>(type: "real", nullable: true),
            //        Dimensions_Weight = table.Column<float>(type: "real", nullable: true),
            //        Barcode_EAN = table.Column<string>(type: "text", nullable: true),
            //        Barcode_UPC = table.Column<string>(type: "text", nullable: true),
            //        ExtendedData_BrickTags = table.Column<string>(type: "text", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("Sets_pkey", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Sets_Box_BoxID",
            //            column: x => x.BoxID,
            //            principalTable: "Box",
            //            principalColumn: "Id");
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Sets_BoxID",
            //    table: "Sets",
            //    column: "BoxID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sets");

            migrationBuilder.DropTable(
                name: "Box");
        }
    }
}
