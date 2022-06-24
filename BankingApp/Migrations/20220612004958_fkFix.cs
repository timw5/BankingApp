using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingApp.Migrations
{
    public partial class fkFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LoginID",
                table: "Accounts",
                oldClrType: typeof(int),
                oldType: "int",
     
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Accounts");
        }
    }
}
