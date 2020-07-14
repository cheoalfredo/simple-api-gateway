using Microsoft.EntityFrameworkCore.Migrations;

namespace K8SCore.Migrations
{
    public partial class Activeenalbledpropadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "sandbox",
                table: "InstanceDetails",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "sandbox",
                table: "InstanceDetails");
        }
    }
}
