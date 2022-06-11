using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingApp.Migrations
{
    public partial class fks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToID",
                table: "Transfers",
                newName: "WithdrawID");

            migrationBuilder.RenameColumn(
                name: "FromID",
                table: "Transfers",
                newName: "WithdrawAccountID");

            migrationBuilder.AlterColumn<int>(
                name: "WithdrawID",
                table: "Transfers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddColumn<int>(
                name: "DepositID",
                table: "Transfers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Deposits",
                table: "Transfers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_Deposits",
                table: "Transfers",
                column: "Deposits");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_WithdrawAccountID",
                table: "Transfers",
                column: "WithdrawAccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Accounts_Deposits",
                table: "Transfers",
                column: "DepositID",
                principalTable: "Accounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Accounts_WithdrawAccountID",
                table: "Transfers",
                column: "WithdrawID",
                principalTable: "Accounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Accounts_Deposits",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Accounts_WithdrawAccountID",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_Deposits",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_WithdrawAccountID",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "DepositID",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "Deposits",
                table: "Transfers");

            migrationBuilder.RenameColumn(
                name: "WithdrawID",
                table: "Transfers",
                newName: "ToID");

            migrationBuilder.RenameColumn(
                name: "WithdrawAccountID",
                table: "Transfers",
                newName: "FromID");

            migrationBuilder.AlterColumn<int>(
                name: "ToID",
                table: "Transfers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 0);
        }
    }
}
