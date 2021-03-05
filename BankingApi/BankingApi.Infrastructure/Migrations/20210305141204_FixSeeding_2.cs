using Microsoft.EntityFrameworkCore.Migrations;

namespace BankingApi.Infrastructure.Migrations
{
    public partial class FixSeeding_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "tbl_Account",
                keyColumn: "AccountId",
                keyValue: 23456,
                column: "MemberId",
                value: 234789);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "tbl_Account",
                keyColumn: "AccountId",
                keyValue: 23456,
                column: "MemberId",
                value: 0);
        }
    }
}
