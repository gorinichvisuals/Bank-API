using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank_API.DataAccessLayer.Migrations
{
    public partial class FixProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "users",
                newName: "phone");

            migrationBuilder.RenameIndex(
                name: "ix_users_phone_number",
                table: "users",
                newName: "ix_users_phone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "phone",
                table: "users",
                newName: "phone_number");

            migrationBuilder.RenameIndex(
                name: "ix_users_phone",
                table: "users",
                newName: "ix_users_phone_number");
        }
    }
}
