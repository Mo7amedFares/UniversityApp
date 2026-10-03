using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchemaToMatchContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Payments");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Services",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "GatewayTransactionId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "GatewayTransactionId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Payments");

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
