using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APBD_Projekt.Migrations
{
    /// <inheritdoc />
    public partial class AddOptionalDiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Discounts_IdDiscount",
                table: "Contracts");

            migrationBuilder.AlterColumn<int>(
                name: "IdDiscount",
                table: "Contracts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Discounts_IdDiscount",
                table: "Contracts",
                column: "IdDiscount",
                principalTable: "Discounts",
                principalColumn: "IdDiscount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Discounts_IdDiscount",
                table: "Contracts");

            migrationBuilder.AlterColumn<int>(
                name: "IdDiscount",
                table: "Contracts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Discounts_IdDiscount",
                table: "Contracts",
                column: "IdDiscount",
                principalTable: "Discounts",
                principalColumn: "IdDiscount",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
