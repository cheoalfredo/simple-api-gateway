using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace K8SCore.Migrations
{
    public partial class InitialBootstrap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "sandbox");

            migrationBuilder.CreateTable(
                name: "InstanceDetails",
                schema: "sandbox",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DeploymentIdentifier = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    PortalUser = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    LastModifiedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstanceDetails", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstanceDetails",
                schema: "sandbox");
        }
    }
}
