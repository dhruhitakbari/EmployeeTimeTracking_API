using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeTimeTracking_API.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserForeingkeyandAddDesiganationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Designation",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "DesignationId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Designation",
                columns: table => new
                {
                    DesignationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Designation", x => x.DesignationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_DesignationId",
                table: "Users",
                column: "DesignationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Designation_DesignationId",
                table: "Users",
                column: "DesignationId",
                principalTable: "Designation",
                principalColumn: "DesignationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Designation_DesignationId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Designation");

            migrationBuilder.DropIndex(
                name: "IX_Users_DesignationId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DesignationId",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Designation",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
