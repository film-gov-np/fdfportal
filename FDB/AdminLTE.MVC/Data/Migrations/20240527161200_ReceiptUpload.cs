using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminLTE.MVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReceiptUpload : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReceiptUploads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankBranchName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoucherNo = table.Column<long>(type: "bigint", nullable: false),
                    AccountNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountOperatingOffice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChequeDraftNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountInWord = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vapata = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepositoryOfficeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepositoryOfficeCode = table.Column<long>(type: "bigint", nullable: false),
                    DepositorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepositorPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionDateNep = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PanNo = table.Column<long>(type: "bigint", nullable: false),
                    VoucherImgFront = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoucherImgBack = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignatureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TheatreId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptUploads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptUploads_Theatres_TheatreId",
                        column: x => x.TheatreId,
                        principalTable: "Theatres",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptUploads_TheatreId",
                table: "ReceiptUploads",
                column: "TheatreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReceiptUploads");
        }
    }
}
