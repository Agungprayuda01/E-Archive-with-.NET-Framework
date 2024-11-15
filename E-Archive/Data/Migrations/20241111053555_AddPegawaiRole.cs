using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Archive.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPegawaiRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "pegawais",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "pegawais");
        }
    }
}
