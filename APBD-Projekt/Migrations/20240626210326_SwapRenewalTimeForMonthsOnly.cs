using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APBD_Projekt.Migrations
{
    /// <inheritdoc />
    public partial class SwapRenewalTimeForMonthsOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubscriptionOffers_RenewalTimes_IdRenewalTime",
                table: "SubscriptionOffers");

            migrationBuilder.DropTable(
                name: "RenewalTimes");

            migrationBuilder.DropIndex(
                name: "IX_SubscriptionOffers_IdRenewalTime",
                table: "SubscriptionOffers");

            migrationBuilder.RenameColumn(
                name: "IdRenewalTime",
                table: "SubscriptionOffers",
                newName: "MonthsPerRenewalTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MonthsPerRenewalTime",
                table: "SubscriptionOffers",
                newName: "IdRenewalTime");

            migrationBuilder.CreateTable(
                name: "RenewalTimes",
                columns: table => new
                {
                    IdRenewalTime = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Months = table.Column<int>(type: "int", nullable: false),
                    Years = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RenewalTimes", x => x.IdRenewalTime);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionOffers_IdRenewalTime",
                table: "SubscriptionOffers",
                column: "IdRenewalTime");

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionOffers_RenewalTimes_IdRenewalTime",
                table: "SubscriptionOffers",
                column: "IdRenewalTime",
                principalTable: "RenewalTimes",
                principalColumn: "IdRenewalTime",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
