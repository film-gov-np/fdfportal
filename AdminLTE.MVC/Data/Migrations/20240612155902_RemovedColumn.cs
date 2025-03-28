using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminLTE.MVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "MovieMVCs");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ReleaseDate",
                table: "MovieMVCs",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ReleaseDate",
                table: "MovieMVCs",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsDeleted",
                table: "MovieMVCs",
                type: "datetime2",
                nullable: true);
        }
    }
}
