using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class removeDaoTaoBoiDuong1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuaTrinhBoiDuong",
                schema: "NS");

            migrationBuilder.DropTable(
                name: "QuaTrinhDaoTao",
                schema: "NS");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 9, 9, 20, 24, 16, 428, DateTimeKind.Local).AddTicks(6539),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 9, 9, 20, 11, 49, 308, DateTimeKind.Local).AddTicks(2271));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 9, 9, 20, 24, 16, 428, DateTimeKind.Local).AddTicks(4017),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 9, 9, 20, 11, 49, 307, DateTimeKind.Local).AddTicks(8811));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 9, 9, 20, 11, 49, 308, DateTimeKind.Local).AddTicks(2271),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 9, 9, 20, 24, 16, 428, DateTimeKind.Local).AddTicks(6539));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 9, 9, 20, 11, 49, 307, DateTimeKind.Local).AddTicks(8811),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 9, 9, 20, 24, 16, 428, DateTimeKind.Local).AddTicks(4017));

            migrationBuilder.CreateTable(
                name: "QuaTrinhBoiDuong",
                schema: "NS",
                columns: table => new
                {
                    IDQuaTrinhBoiDuong = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDCanBo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaHinhThucDaoTao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FileDinhKem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NoiBoiDuong = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuaTrinhBoiDuong", x => x.IDQuaTrinhBoiDuong);
                    table.ForeignKey(
                        name: "FK_QuaTrinhBoiDuong_CanBo",
                        column: x => x.IDCanBo,
                        principalSchema: "NS",
                        principalTable: "CanBo",
                        principalColumn: "IDCanBo");
                    table.ForeignKey(
                        name: "FK_QuaTrinhBoiDuong_HinhThucDaoTao",
                        column: x => x.MaHinhThucDaoTao,
                        principalSchema: "tMasterData",
                        principalTable: "HinhThucDaoTao",
                        principalColumn: "MaHinhThucDaoTao");
                });

            migrationBuilder.CreateTable(
                name: "QuaTrinhDaoTao",
                schema: "NS",
                columns: table => new
                {
                    IDQuaTrinhDaoTao = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDCanBo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaChuyenNganh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaHinhThucDaoTao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaLoaiBangCap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CoSoDaoTao = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FileDinhKem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LuanAnTN = table.Column<bool>(type: "bit", nullable: true),
                    NgayTotNghiep = table.Column<DateTime>(type: "datetime2", maxLength: 10, nullable: false),
                    QuocGia = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuaTrinhDaoTao", x => x.IDQuaTrinhDaoTao);
                    table.ForeignKey(
                        name: "FK_QuaTrinhDaoTao_CanBo",
                        column: x => x.IDCanBo,
                        principalSchema: "NS",
                        principalTable: "CanBo",
                        principalColumn: "IDCanBo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuaTrinhDaoTao_ChuyenNganh",
                        column: x => x.MaChuyenNganh,
                        principalSchema: "tMasterData",
                        principalTable: "ChuyenNganh",
                        principalColumn: "MaChuyenNganh");
                    table.ForeignKey(
                        name: "FK_QuaTrinhDaoTao_HinhThucDaoTao",
                        column: x => x.MaHinhThucDaoTao,
                        principalSchema: "tMasterData",
                        principalTable: "HinhThucDaoTao",
                        principalColumn: "MaHinhThucDaoTao");
                    table.ForeignKey(
                        name: "FK_QuaTrinhDaoTao_LoaiBangCap",
                        column: x => x.MaLoaiBangCap,
                        principalSchema: "tMasterData",
                        principalTable: "LoaiBangCap",
                        principalColumn: "MaLoaiBangCap");
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuaTrinhBoiDuong_IDCanBo",
                schema: "NS",
                table: "QuaTrinhBoiDuong",
                column: "IDCanBo");

            migrationBuilder.CreateIndex(
                name: "IX_QuaTrinhBoiDuong_MaHinhThucDaoTao",
                schema: "NS",
                table: "QuaTrinhBoiDuong",
                column: "MaHinhThucDaoTao");

            migrationBuilder.CreateIndex(
                name: "IX_QuaTrinhDaoTao_IDCanBo",
                schema: "NS",
                table: "QuaTrinhDaoTao",
                column: "IDCanBo");

            migrationBuilder.CreateIndex(
                name: "IX_QuaTrinhDaoTao_MaChuyenNganh",
                schema: "NS",
                table: "QuaTrinhDaoTao",
                column: "MaChuyenNganh");

            migrationBuilder.CreateIndex(
                name: "IX_QuaTrinhDaoTao_MaHinhThucDaoTao",
                schema: "NS",
                table: "QuaTrinhDaoTao",
                column: "MaHinhThucDaoTao");

            migrationBuilder.CreateIndex(
                name: "IX_QuaTrinhDaoTao_MaLoaiBangCap",
                schema: "NS",
                table: "QuaTrinhDaoTao",
                column: "MaLoaiBangCap");
        }
    }
}
