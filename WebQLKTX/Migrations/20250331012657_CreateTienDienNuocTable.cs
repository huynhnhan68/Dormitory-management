using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyKTX.Migrations
{
    public partial class CreateTienDienNuocTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TienDienNuocs",
                columns: table => new
                {
                    MaPhieu = table.Column<int>(type: "int", nullable: false), // Không có Identity
                    MaPhong = table.Column<int>(type: "int", nullable: false),
                    NgayTaoPhieu = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoDien = table.Column<int>(type: "int", nullable: false),
                    GiaDien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoNuoc = table.Column<int>(type: "int", nullable: false),
                    GiaNuoc = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TienDienNuocs", x => x.MaPhieu);
                    table.ForeignKey(
                        name: "FK_TienDienNuocs_Phongs_MaPhong",
                        column: x => x.MaPhong,
                        principalTable: "Phongs",
                        principalColumn: "MaPhong",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TienDienNuocs_MaPhong",
                table: "TienDienNuocs",
                column: "MaPhong");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "TienDienNuocs");
        }
    }
}
