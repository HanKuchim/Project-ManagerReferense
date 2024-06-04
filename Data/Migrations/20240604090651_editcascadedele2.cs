using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_Manager.Data.Migrations
{
    /// <inheritdoc />
    public partial class editcascadedele2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_AspNetUsers_UserId",
                table: "ProjectMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_ProjectMembers_ProjectMemberId",
                table: "TaskAssignments");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMembers_AspNetUsers_UserId",
                table: "ProjectMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignments_ProjectMembers_ProjectMemberId",
                table: "TaskAssignments",
                column: "ProjectMemberId",
                principalTable: "ProjectMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_AspNetUsers_UserId",
                table: "ProjectMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_ProjectMembers_ProjectMemberId",
                table: "TaskAssignments");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMembers_AspNetUsers_UserId",
                table: "ProjectMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignments_ProjectMembers_ProjectMemberId",
                table: "TaskAssignments",
                column: "ProjectMemberId",
                principalTable: "ProjectMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
