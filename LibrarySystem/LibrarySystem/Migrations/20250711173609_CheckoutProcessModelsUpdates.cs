using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarySystem.Migrations
{
    /// <inheritdoc />
    public partial class CheckoutProcessModelsUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookOrder_Book_BookID",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_BookOrder_BookID",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Order",
                newName: "PaymentMethodId");

            migrationBuilder.RenameColumn(
                name: "BookID",
                table: "Order",
                newName: "DeliveryOption");

            migrationBuilder.AddColumn<int>(
                name: "DeliveryAddressId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PickupPointId",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Order",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "BookOrderItem",
                columns: table => new
                {
                    BookOrderItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookOrderId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookOrderItem", x => x.BookOrderItemId);
                    table.ForeignKey(
                        name: "FK_BookOrderItem_BookOrder_BookOrderId",
                        column: x => x.BookOrderId,
                        principalTable: "Order",
                        principalColumn: "BookOrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookOrderItem_Book_BookId",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressLine1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressLine2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryAddress", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookOrder_DeliveryAddressId",
                table: "Order",
                column: "DeliveryAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_BookOrder_PickupPointId",
                table: "Order",
                column: "PickupPointId");

            migrationBuilder.CreateIndex(
                name: "IX_BookOrderItem_BookId",
                table: "BookOrderItem",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookOrderItem_BookOrderId",
                table: "BookOrderItem",
                column: "BookOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookOrder_DeliveryAddress_DeliveryAddressId",
                table: "Order",
                column: "DeliveryAddressId",
                principalTable: "DeliveryAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookOrder_PickupPoint_PickupPointId",
                table: "Order",
                column: "PickupPointId",
                principalTable: "PickupPoint",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookOrder_DeliveryAddress_DeliveryAddressId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_BookOrder_PickupPoint_PickupPointId",
                table: "Order");

            migrationBuilder.DropTable(
                name: "BookOrderItem");

            migrationBuilder.DropTable(
                name: "DeliveryAddress");

            migrationBuilder.DropIndex(
                name: "IX_BookOrder_DeliveryAddressId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_BookOrder_PickupPointId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DeliveryAddressId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PickupPointId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "PaymentMethodId",
                table: "Order",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "DeliveryOption",
                table: "Order",
                newName: "BookID");

            migrationBuilder.CreateIndex(
                name: "IX_BookOrder_BookID",
                table: "Order",
                column: "BookID");

            migrationBuilder.AddForeignKey(
                name: "FK_BookOrder_Book_BookID",
                table: "Order",
                column: "BookID",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
