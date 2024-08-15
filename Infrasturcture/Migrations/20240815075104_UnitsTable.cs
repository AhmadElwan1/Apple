using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UnitsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRules_Tenants_TenantId",
                table: "LeaveRules");

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "Employees",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Countries",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Units_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UnitId",
                table: "Employees",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_TenantId",
                table: "Units",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Units_UnitId",
                table: "Employees",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRules_Tenants_TenantId",
                table: "LeaveRules",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Units_UnitId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRules_Tenants_TenantId",
                table: "LeaveRules");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Employees_UnitId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Employees");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Countries",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRules_Tenants_TenantId",
                table: "LeaveRules",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
