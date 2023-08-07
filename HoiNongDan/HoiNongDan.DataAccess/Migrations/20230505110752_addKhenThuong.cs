using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class addKhenThuong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 5, 18, 7, 52, 662, DateTimeKind.Local).AddTicks(4600),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 4, 10, 40, 49, 457, DateTimeKind.Local).AddTicks(3019));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 5, 18, 7, 52, 662, DateTimeKind.Local).AddTicks(723),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 4, 10, 40, 49, 456, DateTimeKind.Local).AddTicks(8669));

            migrationBuilder.CreateTable(
                name: "DanhHieuKhenThuong",
                schema: "tMasterData",
                columns: table => new
                {
                    MaDanhHieuKhenThuong = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenDanhHieuKhenThuong = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhHieuKhenThuong", x => x.MaDanhHieuKhenThuong);
                });

            migrationBuilder.CreateTable(
                name: "HinhThucKhenThuong",
                schema: "tMasterData",
                columns: table => new
                {
                    MaHinhThucKhenThuong = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenHinhThucKhenThuong = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HinhThucKhenThuong", x => x.MaHinhThucKhenThuong);
                });

            migrationBuilder.CreateTable(
                name: "QuaTrinhKhenThuong",
                schema: "NS",
                columns: table => new
                {
                    IDQuaTrinhKhenThuong = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDCanBo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaHinhThucKhenThuong = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    MaDanhHieuKhenThuong = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    NgayQuyetDinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoQuyetDinh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NguoiKy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    LyDo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(550)", maxLength: 550, nullable: true),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuaTrinhKhenThuong", x => x.IDQuaTrinhKhenThuong);
                    table.ForeignKey(
                        name: "FK_QuanHeGiaDinh_DanhHieuKhenThuong",
                        column: x => x.MaDanhHieuKhenThuong,
                        principalSchema: "tMasterData",
                        principalTable: "DanhHieuKhenThuong",
                        principalColumn: "MaDanhHieuKhenThuong",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuanHeGiaDinh_HinhThucKhenThuong",
                        column: x => x.MaHinhThucKhenThuong,
                        principalSchema: "tMasterData",
                        principalTable: "HinhThucKhenThuong",
                        principalColumn: "MaHinhThucKhenThuong",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuaTrinhKhenThuong_CanBo",
                        column: x => x.IDCanBo,
                        principalSchema: "NS",
                        principalTable: "CanBo",
                        principalColumn: "IDCanBo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuaTrinhKhenThuong_IDCanBo",
                schema: "NS",
                table: "QuaTrinhKhenThuong",
                column: "IDCanBo");

            migrationBuilder.CreateIndex(
                name: "IX_QuaTrinhKhenThuong_MaDanhHieuKhenThuong",
                schema: "NS",
                table: "QuaTrinhKhenThuong",
                column: "MaDanhHieuKhenThuong");

            migrationBuilder.CreateIndex(
                name: "IX_QuaTrinhKhenThuong_MaHinhThucKhenThuong",
                schema: "NS",
                table: "QuaTrinhKhenThuong",
                column: "MaHinhThucKhenThuong");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuaTrinhKhenThuong",
                schema: "NS");

            migrationBuilder.DropTable(
                name: "DanhHieuKhenThuong",
                schema: "tMasterData");

            migrationBuilder.DropTable(
                name: "HinhThucKhenThuong",
                schema: "tMasterData");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 4, 10, 40, 49, 457, DateTimeKind.Local).AddTicks(3019),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 5, 18, 7, 52, 662, DateTimeKind.Local).AddTicks(4600));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 4, 10, 40, 49, 456, DateTimeKind.Local).AddTicks(8669),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 5, 18, 7, 52, 662, DateTimeKind.Local).AddTicks(723));
        }
    }
}
