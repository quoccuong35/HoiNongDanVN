using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portal.DataAccess.Migrations
{
    public partial class addCanBo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "NS");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 22, 16, 17, 0, 409, DateTimeKind.Local).AddTicks(7553),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 22, 15, 23, 22, 260, DateTimeKind.Local).AddTicks(1865));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 22, 16, 17, 0, 409, DateTimeKind.Local).AddTicks(2759),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 22, 15, 23, 22, 259, DateTimeKind.Local).AddTicks(7999));

            migrationBuilder.CreateTable(
                name: "CanBo",
                schema: "NS",
                columns: table => new
                {
                    IDCanBo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaCaBo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HoDem = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NgaySinh = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    ThangSinh = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    NamSinh = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    GioiTinh = table.Column<int>(type: "int", nullable: false),
                    SoCCCD = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NgayCapCCCD = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaTinhTrang = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IdCoSo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdDepartment = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaChucVu = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MaNgachLuong = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaBacLuong = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HeSoLuong = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NgayNangBacLuong = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PhuCapChucVu = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PhuCapVuotKhung = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PhuCapKiemNhiem = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PhuCapKhuVuc = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LuongKhoan = table.Column<int>(type: "int", nullable: true),
                    NgayVaoBienChe = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayThamGiaCongTac = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaHeDaoTao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaTrinhDoHocVan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ChuyenNganh = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MaTrinhDoTinHoc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaTrinhDoNgoaiNgu = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaTrinhDoChinhTri = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaHocHam = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaDanToc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaTonGiao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NoiSinh = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ChoOHienNay = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    NgayvaoDangDuBi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayVaoDangChinhThuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    HinhAnh = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CanBo", x => x.IDCanBo);
                    table.ForeignKey(
                        name: "FK_CanBo_BacLuong",
                        column: x => x.MaBacLuong,
                        principalSchema: "tMasterData",
                        principalTable: "BacLuongModel",
                        principalColumn: "MaBacLuong");
                    table.ForeignKey(
                        name: "FK_CanBo_ChucVu",
                        column: x => x.MaChucVu,
                        principalSchema: "tMasterData",
                        principalTable: "ChucVuModel",
                        principalColumn: "MaChucVu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CanBo_CoSo",
                        column: x => x.IdCoSo,
                        principalSchema: "tMasterData",
                        principalTable: "CoSoModel",
                        principalColumn: "IdCoSo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CanBo_DanToc",
                        column: x => x.MaDanToc,
                        principalSchema: "tMasterData",
                        principalTable: "DanTocModel",
                        principalColumn: "MaDanToc",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CanBo_Department",
                        column: x => x.IdDepartment,
                        principalSchema: "tMasterData",
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CanBo_TonGiao",
                        column: x => x.MaTonGiao,
                        principalSchema: "tMasterData",
                        principalTable: "TonGiaoModel",
                        principalColumn: "MaTonGiao",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CanBo_TrinhDoChinhTri",
                        column: x => x.MaTrinhDoChinhTri,
                        principalSchema: "tMasterData",
                        principalTable: "TrinhDoChinhTriModel",
                        principalColumn: "MaTrinhDoChinhTri");
                    table.ForeignKey(
                        name: "FK_CanBo_TrinhDoHocVan",
                        column: x => x.MaTrinhDoHocVan,
                        principalSchema: "tMasterData",
                        principalTable: "TrinhDoHocVanModel",
                        principalColumn: "MaTrinhDoHocVan",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CanBo_TrinhDoNgoaiNgu",
                        column: x => x.MaTrinhDoNgoaiNgu,
                        principalSchema: "tMasterData",
                        principalTable: "TrinhDoNgoaiNguModel",
                        principalColumn: "MaTrinhDoNgoaiNgu");
                    table.ForeignKey(
                        name: "FK_CanBo_TrinhDoTinHoc",
                        column: x => x.MaTrinhDoTinHoc,
                        principalSchema: "tMasterData",
                        principalTable: "TrinhDoTinHocModel",
                        principalColumn: "MaTrinhDoTinHoc");
                    table.ForeignKey(
                        name: "FKg_CanBo_TinhTrang",
                        column: x => x.MaTinhTrang,
                        principalSchema: "tMasterData",
                        principalTable: "TinhTrangModel",
                        principalColumn: "MaTinhTrang",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_IdCoSo",
                schema: "NS",
                table: "CanBo",
                column: "IdCoSo");

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_IdDepartment",
                schema: "NS",
                table: "CanBo",
                column: "IdDepartment");

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_MaBacLuong",
                schema: "NS",
                table: "CanBo",
                column: "MaBacLuong");

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_MaChucVu",
                schema: "NS",
                table: "CanBo",
                column: "MaChucVu");

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_MaDanToc",
                schema: "NS",
                table: "CanBo",
                column: "MaDanToc");

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_MaTinhTrang",
                schema: "NS",
                table: "CanBo",
                column: "MaTinhTrang");

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_MaTonGiao",
                schema: "NS",
                table: "CanBo",
                column: "MaTonGiao");

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_MaTrinhDoChinhTri",
                schema: "NS",
                table: "CanBo",
                column: "MaTrinhDoChinhTri");

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_MaTrinhDoHocVan",
                schema: "NS",
                table: "CanBo",
                column: "MaTrinhDoHocVan");

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_MaTrinhDoNgoaiNgu",
                schema: "NS",
                table: "CanBo",
                column: "MaTrinhDoNgoaiNgu");

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_MaTrinhDoTinHoc",
                schema: "NS",
                table: "CanBo",
                column: "MaTrinhDoTinHoc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CanBo",
                schema: "NS");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 22, 15, 23, 22, 260, DateTimeKind.Local).AddTicks(1865),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 22, 16, 17, 0, 409, DateTimeKind.Local).AddTicks(7553));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 22, 15, 23, 22, 259, DateTimeKind.Local).AddTicks(7999),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 22, 16, 17, 0, 409, DateTimeKind.Local).AddTicks(2759));
        }
    }
}
