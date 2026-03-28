using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projects_Management_Studio.Infra.Migrations
{
    /// <inheritdoc />
    public partial class fixFieldNameCreatedAtforproject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GeatedAt",
                table: "Projects",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Projects",
                newName: "GeatedAt");
        }
    }
}
