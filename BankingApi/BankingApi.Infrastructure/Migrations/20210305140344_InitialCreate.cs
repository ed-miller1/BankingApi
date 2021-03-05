using Microsoft.EntityFrameworkCore.Migrations;

namespace BankingApi.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_Institution",
                columns: table => new
                {
                    InstitutionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InstitutionName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsitutionId", x => x.InstitutionId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Member",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GivenName = table.Column<string>(type: "TEXT", nullable: true),
                    Surname = table.Column<string>(type: "TEXT", nullable: true),
                    InstitutionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberId", x => x.MemberId);
                    table.ForeignKey(
                        name: "FK_tbl_Member_tbl_Institution_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "tbl_Institution",
                        principalColumn: "InstitutionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Account",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Balance = table.Column<double>(type: "REAL", nullable: false),
                    MemberId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountId", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_tbl_Account_tbl_Member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "tbl_Member",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "tbl_Institution",
                columns: new[] { "InstitutionId", "InstitutionName" },
                values: new object[] { 78923, "First Credit Union" });

            migrationBuilder.InsertData(
                table: "tbl_Member",
                columns: new[] { "MemberId", "GivenName", "InstitutionId", "Surname" },
                values: new object[] { 234789, "John", 78923, "Doe" });

            migrationBuilder.InsertData(
                table: "tbl_Account",
                columns: new[] { "AccountId", "Balance", "MemberId" },
                values: new object[] { 23456, 12.5, 234789 });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Account_MemberId",
                table: "tbl_Account",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Member_InstitutionId",
                table: "tbl_Member",
                column: "InstitutionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_Account");

            migrationBuilder.DropTable(
                name: "tbl_Member");

            migrationBuilder.DropTable(
                name: "tbl_Institution");
        }
    }
}
