using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Intranet.DataAccess.Data.Migrations.CorpComm
{
    public partial class RemoveUnusedColumnInOrderHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemReceipt",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "PaymentDueDate",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "TrackingNumber",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "OrderHeaders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ItemReceipt",
                table: "OrderHeaders",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "OrderHeaders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDueDate",
                table: "OrderHeaders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrackingNumber",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
