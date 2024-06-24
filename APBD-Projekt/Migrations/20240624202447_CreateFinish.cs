using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APBD_Projekt.Migrations
{
    /// <inheritdoc />
    public partial class CreateFinish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RenewalTime",
                columns: table => new
                {
                    IdRenewalTime = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Months = table.Column<int>(type: "int", nullable: false),
                    Years = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RenewalTime", x => x.IdRenewalTime);
                });

            migrationBuilder.CreateTable(
                name: "SoftwareVersion",
                columns: table => new
                {
                    IdSoftwareVersion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdSoftware = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwareVersion", x => x.IdSoftwareVersion);
                    table.ForeignKey(
                        name: "FK_SoftwareVersion_Software_IdSoftware",
                        column: x => x.IdSoftware,
                        principalTable: "Software",
                        principalColumn: "IdSoftware",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionOffer",
                columns: table => new
                {
                    IdSubscriptionOffer = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdSoftware = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    IdRenewalTime = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionOffer", x => x.IdSubscriptionOffer);
                    table.ForeignKey(
                        name: "FK_SubscriptionOffer_RenewalTime_IdRenewalTime",
                        column: x => x.IdRenewalTime,
                        principalTable: "RenewalTime",
                        principalColumn: "IdRenewalTime",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubscriptionOffer_Software_IdSoftware",
                        column: x => x.IdSoftware,
                        principalTable: "Software",
                        principalColumn: "IdSoftware",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contract",
                columns: table => new
                {
                    IdContract = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdClient = table.Column<int>(type: "int", nullable: false),
                    IdSoftwareVersion = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    YearsOfSupport = table.Column<int>(type: "int", nullable: false),
                    FinalPrice = table.Column<decimal>(type: "money", nullable: false),
                    IdDiscount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract", x => x.IdContract);
                    table.ForeignKey(
                        name: "FK_Contract_Client_IdClient",
                        column: x => x.IdClient,
                        principalTable: "Client",
                        principalColumn: "IdClient",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contract_Discount_IdDiscount",
                        column: x => x.IdDiscount,
                        principalTable: "Discount",
                        principalColumn: "IdDiscount");
                    table.ForeignKey(
                        name: "FK_Contract_SoftwareVersion_IdSoftwareVersion",
                        column: x => x.IdSoftwareVersion,
                        principalTable: "SoftwareVersion",
                        principalColumn: "IdSoftwareVersion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    IdSubscription = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdClient = table.Column<int>(type: "int", nullable: false),
                    IdSubscriptionOffer = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IdDiscount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.IdSubscription);
                    table.ForeignKey(
                        name: "FK_Subscription_Client_IdClient",
                        column: x => x.IdClient,
                        principalTable: "Client",
                        principalColumn: "IdClient",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscription_Discount_IdDiscount",
                        column: x => x.IdDiscount,
                        principalTable: "Discount",
                        principalColumn: "IdDiscount");
                    table.ForeignKey(
                        name: "FK_Subscription_SubscriptionOffer_IdSubscriptionOffer",
                        column: x => x.IdSubscriptionOffer,
                        principalTable: "SubscriptionOffer",
                        principalColumn: "IdSubscriptionOffer",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractPayment",
                columns: table => new
                {
                    ContractPayment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdContract = table.Column<int>(type: "int", nullable: false),
                    PaymentAmount = table.Column<decimal>(type: "money", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractPayment", x => x.ContractPayment);
                    table.ForeignKey(
                        name: "FK_ContractPayment_Contract_IdContract",
                        column: x => x.IdContract,
                        principalTable: "Contract",
                        principalColumn: "IdContract",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionPayment",
                columns: table => new
                {
                    IdSubscriptionPayment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdSubscription = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPayment", x => x.IdSubscriptionPayment);
                    table.ForeignKey(
                        name: "FK_SubscriptionPayment_Subscription_IdSubscription",
                        column: x => x.IdSubscription,
                        principalTable: "Subscription",
                        principalColumn: "IdSubscription",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contract_IdClient",
                table: "Contract",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_IdDiscount",
                table: "Contract",
                column: "IdDiscount");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_IdSoftwareVersion",
                table: "Contract",
                column: "IdSoftwareVersion");

            migrationBuilder.CreateIndex(
                name: "IX_ContractPayment_IdContract",
                table: "ContractPayment",
                column: "IdContract");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareVersion_IdSoftware",
                table: "SoftwareVersion",
                column: "IdSoftware");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_IdClient",
                table: "Subscription",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_IdDiscount",
                table: "Subscription",
                column: "IdDiscount");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_IdSubscriptionOffer",
                table: "Subscription",
                column: "IdSubscriptionOffer");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionOffer_IdRenewalTime",
                table: "SubscriptionOffer",
                column: "IdRenewalTime");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionOffer_IdSoftware",
                table: "SubscriptionOffer",
                column: "IdSoftware");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPayment_IdSubscription",
                table: "SubscriptionPayment",
                column: "IdSubscription");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractPayment");

            migrationBuilder.DropTable(
                name: "SubscriptionPayment");

            migrationBuilder.DropTable(
                name: "Contract");

            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropTable(
                name: "SoftwareVersion");

            migrationBuilder.DropTable(
                name: "SubscriptionOffer");

            migrationBuilder.DropTable(
                name: "RenewalTime");
        }
    }
}
