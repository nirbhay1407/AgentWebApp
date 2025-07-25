using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ioc.Data.Migrations
{
    /// <inheritdoc />
    public partial class SiteInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SiteSetup",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    SiteName = table.Column<string>(type: "text", nullable: true),
                    SiteUrl = table.Column<string>(type: "text", nullable: true),
                    SiteLogo = table.Column<string>(type: "text", nullable: true),
                    SiteFavicon = table.Column<string>(type: "text", nullable: true),
                    SiteDescription = table.Column<string>(type: "text", nullable: true),
                    SiteKeywords = table.Column<string>(type: "text", nullable: true),
                    SiteEmail = table.Column<string>(type: "text", nullable: true),
                    SitePhone = table.Column<string>(type: "text", nullable: true),
                    SiteAddress = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    SiteCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteSetup", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiteSetup");
        }
    }
}
