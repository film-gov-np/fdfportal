using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminLTE.MVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class BrandMovieThreator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BrandCode",
                table: "Theatres",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Theatres",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PANNumber",
                table: "Theatres",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RegNumber",
                table: "Theatres",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TheatreId",
                table: "Theatres",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "VATNumber",
                table: "Theatres",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BrandMVCs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApiUsername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApiPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusValue = table.Column<int>(type: "int", nullable: true),
                    IsTest = table.Column<bool>(type: "bit", nullable: true),
                    AddedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsModified = table.Column<bool>(type: "bit", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    BrandID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandMVCs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MovieMVCs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieID = table.Column<int>(type: "int", nullable: true),
                    MovieCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GradeCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductionHouseCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductionType = table.Column<int>(type: "int", nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusValue = table.Column<int>(type: "int", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsModified = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CanSpecifyMovieName = table.Column<bool>(type: "bit", nullable: true),
                    SendMovieCodesToExhibitorEmail = table.Column<bool>(type: "bit", nullable: true),
                    SendNotificationToProducers = table.Column<bool>(type: "bit", nullable: true),
                    SendNotificationToDistributors = table.Column<bool>(type: "bit", nullable: true),
                    AddedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieMVCs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrandMVCs");

            migrationBuilder.DropTable(
                name: "MovieMVCs");

            migrationBuilder.DropColumn(
                name: "BrandCode",
                table: "Theatres");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Theatres");

            migrationBuilder.DropColumn(
                name: "PANNumber",
                table: "Theatres");

            migrationBuilder.DropColumn(
                name: "RegNumber",
                table: "Theatres");

            migrationBuilder.DropColumn(
                name: "TheatreId",
                table: "Theatres");

            migrationBuilder.DropColumn(
                name: "VATNumber",
                table: "Theatres");
        }
    }
}
