using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminLTE.MVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class IRDTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IRDOfficeId",
                table: "Theatres",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IRDOffices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankBranchName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountOperatingOffice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PANNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VATNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IRDOffices", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Theatres_IRDOfficeId",
                table: "Theatres",
                column: "IRDOfficeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Theatres_IRDOffices_IRDOfficeId",
                table: "Theatres",
                column: "IRDOfficeId",
                principalTable: "IRDOffices",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Theatres_IRDOffices_IRDOfficeId",
                table: "Theatres");

            migrationBuilder.DropTable(
                name: "IRDOffices");

            migrationBuilder.DropIndex(
                name: "IX_Theatres_IRDOfficeId",
                table: "Theatres");

            migrationBuilder.DropColumn(
                name: "IRDOfficeId",
                table: "Theatres");
        }
    }
}
