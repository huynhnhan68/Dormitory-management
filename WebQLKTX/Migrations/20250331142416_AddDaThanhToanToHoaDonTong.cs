using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyKTX.Migrations
{
    /// <inheritdoc />
    public partial class AddDaThanhToanToHoaDonTong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
    name: "HoaDonTongs",
    columns: table => new
    {
        MaHoaDon = table.Column<string>(type: "int ", nullable: false), // int, không Identity
        MaPhieuTienDienNuoc = table.Column<int>(type: "int", nullable: false),
        MaPhong = table.Column<int>(type: "int", nullable: false),
        TienPhong = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
        DaThanhToan = table.Column<bool>(type: "bit", nullable: false)
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_HoaDonTongs", x => x.MaHoaDon);
        table.ForeignKey(
            name: "FK_HoaDonTongs_TienDienNuocs_MaPhieuTienDienNuoc",
            column: x => x.MaPhieuTienDienNuoc,
            principalTable: "TienDienNuocs",
            principalColumn: "MaPhieu",
            onDelete: ReferentialAction.Cascade);
    });


            migrationBuilder.CreateIndex(
                name: "IX_HoaDonTongs_MaPhieuTienDienNuoc",
                table: "HoaDonTongs",
                column: "MaPhieuTienDienNuoc");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HoaDonTongs");
        }
    }
}
