using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microsoft.eShopWeb.Infrastructure.Logging.Migrations
{
    /// <inheritdoc />
    public partial class ExpInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "exp");

            migrationBuilder.CreateTable(
                name: "Assignment",
                schema: "exp",
                columns: table => new
                {
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExperimentName = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    VariantId = table.Column<string>(type: "nvarchar(64)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignment", x => x.SessionId);
                });

            migrationBuilder.CreateTable(
                name: "BasketSessionMapping",
                schema: "exp",
                columns: table => new
                {
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BasketId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketSessionMapping", x => new { x.SessionId, x.BasketId });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assignment",
                schema: "exp");

            migrationBuilder.DropTable(
                name: "BasketSessionMapping",
                schema: "exp");
        }
    }
}
