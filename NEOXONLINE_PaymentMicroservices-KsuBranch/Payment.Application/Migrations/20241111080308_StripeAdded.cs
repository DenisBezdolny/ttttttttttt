using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Payment.Application.Migrations
{
    /// <inheritdoc />
    public partial class StripeAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameColumn(
            //    name: "PhoneNamber",
            //    table: "User",
            //    newName: "PhoneNumber");

            migrationBuilder.CreateTable(
                name: "StripeTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StripeSessionId = table.Column<string>(type: "text", nullable: false),
                    PaymentStatus = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StripeTransaction", x => x.Id);
                });

            //migrationBuilder.CreateTable(
            //    name: "Subscription",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        UserId = table.Column<Guid>(type: "uuid", nullable: false),
            //        ProductId = table.Column<int>(type: "integer", nullable: false),
            //        StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        IsPaid = table.Column<bool>(type: "boolean", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Subscription", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Subscription_Product_ProductId",
            //            column: x => x.ProductId,
            //            principalTable: "Product",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Subscription_ProductId",
            //    table: "Subscription",
            //    column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StripeTransaction");

            //migrationBuilder.DropTable(
            //    name: "Subscription");

            //migrationBuilder.RenameColumn(
            //    name: "PhoneNumber",
            //    table: "User",
            //    newName: "PhoneNamber");
        }
    }
}
