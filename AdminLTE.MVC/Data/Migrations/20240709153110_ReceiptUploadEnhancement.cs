using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminLTE.MVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReceiptUploadEnhancement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FineAmount",
                table: "ReceiptUploads",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "FineVoucherReceipt",
                table: "ReceiptUploads",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GrossCollection",
                table: "ReceiptUploads",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "MonthlySalesReport",
                table: "ReceiptUploads",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NepaliMonth",
                table: "ReceiptUploads",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FineAmount",
                table: "ReceiptUploads");

            migrationBuilder.DropColumn(
                name: "FineVoucherReceipt",
                table: "ReceiptUploads");

            migrationBuilder.DropColumn(
                name: "GrossCollection",
                table: "ReceiptUploads");

            migrationBuilder.DropColumn(
                name: "MonthlySalesReport",
                table: "ReceiptUploads");

            migrationBuilder.DropColumn(
                name: "NepaliMonth",
                table: "ReceiptUploads");
        }
    }
}
