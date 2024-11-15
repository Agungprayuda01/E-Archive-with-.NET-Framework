using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Archive.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialPegawai : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pegawais",
                columns: table => new
                {
                    Nik = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alamat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tanggallahir = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fotopath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pegawais", x => x.Nik);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pegawais");
        }
    }
}
