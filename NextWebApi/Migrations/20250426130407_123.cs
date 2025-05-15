using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextWebApi.Migrations
{
    /// <inheritdoc />
    public partial class _123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Memo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Memo",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
