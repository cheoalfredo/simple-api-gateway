using Microsoft.EntityFrameworkCore.Migrations;

namespace K8SCore.Migrations
{
    public partial class indexforuseraddedtoInstanceDetailsfixed : Migration
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
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_InstanceDetails_PortalUser",
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

            migrationBuilder.DropIndex(
                name: "IX_InstanceDetails_PortalUser",
                schema: "sandbox",
                table: "InstanceDetails");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InstanceDetails",
                schema: "sandbox",
                table: "InstanceDetails",
                column: "PortalUser");
        }
    }
}
