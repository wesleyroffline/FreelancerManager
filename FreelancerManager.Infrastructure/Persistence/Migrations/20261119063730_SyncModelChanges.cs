using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelancerManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SyncModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_Contracts_ContractId",
                table: "Receipts");

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_Contracts_ContractId",
                table: "Receipts",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_Contracts_ContractId",
                table: "Receipts");

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_Contracts_ContractId",
                table: "Receipts",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
