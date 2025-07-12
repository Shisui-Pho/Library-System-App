using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarySystem.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BookOrderItemId",
                table: "OrderItem",
                newName: "OrderItemId");

            migrationBuilder.RenameColumn(
                name: "BookOrderId",
                table: "Order",
                newName: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderItemId",
                table: "OrderItem",
                newName: "BookOrderItemId");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Order",
                newName: "BookOrderId");
        }
    }
}
