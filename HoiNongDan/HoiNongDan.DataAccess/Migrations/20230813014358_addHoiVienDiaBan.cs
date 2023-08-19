using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class addHoiVienDiaBan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiaBanHoatDong_ThanhVien",
                schema: "HV");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 13, 8, 43, 56, 581, DateTimeKind.Local).AddTicks(6811),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 12, 19, 41, 27, 340, DateTimeKind.Local).AddTicks(7379));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 13, 8, 43, 56, 581, DateTimeKind.Local).AddTicks(3342),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 12, 19, 41, 27, 340, DateTimeKind.Local).AddTicks(354));

            migrationBuilder.AddColumn<string>(
                name: "LoaiHoiVien",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MaDiaBanHoatDong",
                schema: "NS",
                table: "CanBo",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThamGia_CLB_DN_MH_HTX_THT",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(800)",
                maxLength: 800,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThamGia_SH_DoanThe_HoiDoanKhac",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(800)",
                maxLength: 800,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThamGia_THNN_CHNN",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(800)",
                maxLength: 800,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_MaDiaBanHoatDong",
                schema: "NS",
                table: "CanBo",
                column: "MaDiaBanHoatDong");

            migrationBuilder.AddForeignKey(
                name: "FK_CanBo_DiaBanHoatDong",
                schema: "NS",
                table: "CanBo",
                column: "MaDiaBanHoatDong",
                principalSchema: "HV",
                principalTable: "DiaBanHoatDong",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_DiaBanHoatDong",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropIndex(
                name: "IX_CanBo_MaDiaBanHoatDong",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "LoaiHoiVien",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "MaDiaBanHoatDong",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "ThamGia_CLB_DN_MH_HTX_THT",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "ThamGia_SH_DoanThe_HoiDoanKhac",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "ThamGia_THNN_CHNN",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 12, 19, 41, 27, 340, DateTimeKind.Local).AddTicks(7379),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 13, 8, 43, 56, 581, DateTimeKind.Local).AddTicks(6811));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 12, 19, 41, 27, 340, DateTimeKind.Local).AddTicks(354),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 13, 8, 43, 56, 581, DateTimeKind.Local).AddTicks(3342));

            migrationBuilder.CreateTable(
                name: "DiaBanHoatDong_ThanhVien",
                schema: "HV",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDCanBo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdDiaBan = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaChucVu = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LyDo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NgayRoiDi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayVao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoiDi = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaBanHoatDong_ThanhVien", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiaBanHoatDong_ThanhVien_CanBo",
                        column: x => x.IDCanBo,
                        principalSchema: "NS",
                        principalTable: "CanBo",
                        principalColumn: "IDCanBo");
                    table.ForeignKey(
                        name: "FK_DiaBanHoatDong_ThanhVien_ChucVu",
                        column: x => x.MaChucVu,
                        principalSchema: "tMasterData",
                        principalTable: "ChucVuModel",
                        principalColumn: "MaChucVu");
                    table.ForeignKey(
                        name: "FK_DiaBanHoatDong_ThanhVien_DiaBanHoatDong",
                        column: x => x.IdDiaBan,
                        principalSchema: "HV",
                        principalTable: "DiaBanHoatDong",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiaBanHoatDong_ThanhVien_IDCanBo",
                schema: "HV",
                table: "DiaBanHoatDong_ThanhVien",
                column: "IDCanBo");

            migrationBuilder.CreateIndex(
                name: "IX_DiaBanHoatDong_ThanhVien_IdDiaBan",
                schema: "HV",
                table: "DiaBanHoatDong_ThanhVien",
                column: "IdDiaBan");

            migrationBuilder.CreateIndex(
                name: "IX_DiaBanHoatDong_ThanhVien_MaChucVu",
                schema: "HV",
                table: "DiaBanHoatDong_ThanhVien",
                column: "MaChucVu");
        }
    }
}
