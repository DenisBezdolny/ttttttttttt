using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payment.Application.Migrations
{
    /// <inheritdoc />
    public partial class MoreFieldsToStripeTransactionAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Amount",
                table: "StripeTransaction",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "ClientIp",
                table: "StripeTransaction",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "StripeTransaction",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "StripeTransaction",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceId",
                table: "StripeTransaction",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentId",
                table: "StripeTransaction",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "StripeTransaction",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusReason",
                table: "StripeTransaction",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "StripeTransaction",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "StripeTransaction");

            migrationBuilder.DropColumn(
                name: "ClientIp",
                table: "StripeTransaction");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "StripeTransaction");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "StripeTransaction");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "StripeTransaction");

            migrationBuilder.DropColumn(
                name: "PaymentIntentId",
                table: "StripeTransaction");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "StripeTransaction");

            migrationBuilder.DropColumn(
                name: "StatusReason",
                table: "StripeTransaction");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "StripeTransaction");
        }
    }
}
