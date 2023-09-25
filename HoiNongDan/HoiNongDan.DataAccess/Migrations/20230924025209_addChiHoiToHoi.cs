using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class addChiHoiToHoi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 9, 24, 9, 52, 8, 622, DateTimeKind.Local).AddTicks(1312),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 9, 21, 20, 7, 6, 380, DateTimeKind.Local).AddTicks(6203));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 9, 24, 9, 52, 8, 621, DateTimeKind.Local).AddTicks(8300),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 9, 21, 20, 7, 6, 380, DateTimeKind.Local).AddTicks(2051));

            migrationBuilder.AlterColumn<string>(
                name: "LoaiHoiVien",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoTroDaoTaoNghe",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoTroKhac",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoTrovayVon",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HoiVienDanhDu",
                schema: "NS",
                table: "CanBo",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HoiVienNongCot",
                schema: "NS",
                table: "CanBo",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoiVienUuTuNam",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MaChiHoi",
                schema: "NS",
                table: "CanBo",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MaToHoi",
                schema: "NS",
                table: "CanBo",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoLuong",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChiHoi",
                schema: "tMasterData",
                columns: table => new
                {
                    MaChiHoi = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenChiHoi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiHoi", x => x.MaChiHoi);
                });

            migrationBuilder.CreateTable(
                name: "ToHoi",
                schema: "tMasterData",
                columns: table => new
                {
                    MaToHoi = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenToHoi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToHoi", x => x.MaToHoi);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_MaChiHoi",
                schema: "NS",
                table: "CanBo",
                column: "MaChiHoi");

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_MaToHoi",
                schema: "NS",
                table: "CanBo",
                column: "MaToHoi");

            migrationBuilder.AddForeignKey(
                name: "FK_CanBo_ChiHoi",
                schema: "NS",
                table: "CanBo",
                column: "MaChiHoi",
                principalSchema: "tMasterData",
                principalTable: "ChiHoi",
                principalColumn: "MaChiHoi");

            migrationBuilder.AddForeignKey(
                name: "FK_CanBo_ToHoi",
                schema: "NS",
                table: "CanBo",
                column: "MaToHoi",
                principalSchema: "tMasterData",
                principalTable: "ToHoi",
                principalColumn: "MaToHoi");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_ChiHoi",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_ToHoi",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropTable(
                name: "ChiHoi",
                schema: "tMasterData");

            migrationBuilder.DropTable(
                name: "ToHoi",
                schema: "tMasterData");

            migrationBuilder.DropIndex(
                name: "IX_CanBo_MaChiHoi",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropIndex(
                name: "IX_CanBo_MaToHoi",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "HoTroDaoTaoNghe",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "HoTroKhac",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "HoTrovayVon",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "HoiVienDanhDu",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "HoiVienNongCot",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "HoiVienUuTuNam",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "MaChiHoi",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "MaToHoi",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "SoLuong",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 9, 21, 20, 7, 6, 380, DateTimeKind.Local).AddTicks(6203),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 9, 24, 9, 52, 8, 622, DateTimeKind.Local).AddTicks(1312));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 9, 21, 20, 7, 6, 380, DateTimeKind.Local).AddTicks(2051),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 9, 24, 9, 52, 8, 621, DateTimeKind.Local).AddTicks(8300));

            migrationBuilder.AlterColumn<string>(
                name: "LoaiHoiVien",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);
        }
    }
}
