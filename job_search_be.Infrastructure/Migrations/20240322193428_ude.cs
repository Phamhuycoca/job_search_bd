using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace job_search_be.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ude : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PermissionId",
                table: "Roles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_PermissionId",
                table: "Roles",
                column: "PermissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Permissions_PermissionId",
                table: "Roles",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "PermissionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Permissions_PermissionId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_PermissionId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "PermissionId",
                table: "Roles");
        }
    }
}
