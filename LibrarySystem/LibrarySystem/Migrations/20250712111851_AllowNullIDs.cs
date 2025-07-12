using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarySystem.Migrations
{
    /// <inheritdoc />
    public partial class AllowNullIDs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookOrder_DeliveryAddress_DeliveryAddressId",
                table: "Order");

            migrationBuilder.AlterColumn<int>(
                name: "DeliveryAddressId",
                table: "Order",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_BookOrder_DeliveryAddress_DeliveryAddressId",
                table: "Order",
                column: "DeliveryAddressId",
                principalTable: "DeliveryAddress",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookOrder_DeliveryAddress_DeliveryAddressId",
                table: "Order");

            migrationBuilder.AlterColumn<int>(
                name: "DeliveryAddressId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BookOrder_DeliveryAddress_DeliveryAddressId",
                table: "Order",
                column: "DeliveryAddressId",
                principalTable: "DeliveryAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
