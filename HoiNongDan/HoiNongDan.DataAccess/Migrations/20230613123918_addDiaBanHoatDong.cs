using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class addDiaBanHoatDong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "HV");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 6, 13, 19, 39, 16, 975, DateTimeKind.Local).AddTicks(5064),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 23, 8, 54, 58, 380, DateTimeKind.Local).AddTicks(7118));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 6, 13, 19, 39, 16, 975, DateTimeKind.Local).AddTicks(301),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 23, 8, 54, 58, 379, DateTimeKind.Local).AddTicks(6808));

            migrationBuilder.CreateTable(
                name: "DiaBanHoatDong",
                schema: "HV",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenDiaBanHoatDong = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MaTinhThanhPho = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaQuanHuyen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaPhuongXa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Actived = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaBanHoatDong", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiaBanHoatDong_PhuongXa",
                        column: x => x.MaPhuongXa,
                        principalSchema: "tMasterData",
                        principalTable: "PhuongXaModel",
                        principalColumn: "MaPhuongXa");
                    table.ForeignKey(
                        name: "FK_DiaBanHoatDong_QuanHuyen",
                        column: x => x.MaQuanHuyen,
                        principalSchema: "tMasterData",
                        principalTable: "QuanHuyenModel",
                        principalColumn: "MaQuanHuyen");
                    table.ForeignKey(
                        name: "FK_DiaBanHoatDong_TinhThanhPho",
                        column: x => x.MaTinhThanhPho,
                        principalSchema: "tMasterData",
                        principalTable: "TinhThanhPhoModel",
                        principalColumn: "MaTinhThanhPho",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiaBanHoatDong_MaPhuongXa",
                schema: "HV",
                table: "DiaBanHoatDong",
                column: "MaPhuongXa");

            migrationBuilder.CreateIndex(
                name: "IX_DiaBanHoatDong_MaQuanHuyen",
                schema: "HV",
                table: "DiaBanHoatDong",
                column: "MaQuanHuyen");

            migrationBuilder.CreateIndex(
                name: "IX_DiaBanHoatDong_MaTinhThanhPho",
                schema: "HV",
                table: "DiaBanHoatDong",
                column: "MaTinhThanhPho");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiaBanHoatDong",
                schema: "HV");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 23, 8, 54, 58, 380, DateTimeKind.Local).AddTicks(7118),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 6, 13, 19, 39, 16, 975, DateTimeKind.Local).AddTicks(5064));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 23, 8, 54, 58, 379, DateTimeKind.Local).AddTicks(6808),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 6, 13, 19, 39, 16, 975, DateTimeKind.Local).AddTicks(301));
        }
    }
}
