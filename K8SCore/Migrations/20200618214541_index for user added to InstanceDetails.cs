using Microsoft.EntityFrameworkCore.Migrations;

namespace K8SCore.Migrations
{
    public partial class indexforuseraddedtoInstanceDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_InstanceDetails",
                schema: "sandbox",
                table: "InstanceDetails");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InstanceDetails",
                schema: "sandbox",
                table: "InstanceDetails",
                column: "PortalUser");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_InstanceDetails",
                schema: "sandbox",
                table: "InstanceDetails");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InstanceDetails",
                schema: "sandbox",
                table: "InstanceDetails",
                column: "Id");
        }
    }
}
