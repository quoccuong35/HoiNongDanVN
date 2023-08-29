using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editGiaDinhThuocDien : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_GiaDinhThuocDien_GiaDinhThuocDienMaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropIndex(
                name: "IX_CanBo_GiaDinhThuocDienMaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "GiaDinhThuocDienMaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 26, 14, 58, 42, 857, DateTimeKind.Local).AddTicks(2382),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 26, 14, 49, 17, 826, DateTimeKind.Local).AddTicks(168));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 26, 14, 58, 42, 856, DateTimeKind.Local).AddTicks(9324),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 26, 14, 49, 17, 825, DateTimeKind.Local).AddTicks(7918));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 26, 14, 49, 17, 826, DateTimeKind.Local).AddTicks(168),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 26, 14, 58, 42, 857, DateTimeKind.Local).AddTicks(2382));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 26, 14, 49, 17, 825, DateTimeKind.Local).AddTicks(7918),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 26, 14, 58, 42, 856, DateTimeKind.Local).AddTicks(9324));

            migrationBuilder.AddColumn<string>(
                name: "GiaDinhThuocDienMaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(10)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_GiaDinhThuocDienMaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo",
                column: "GiaDinhThuocDienMaGiaDinhThuocDien");

            migrationBuilder.AddForeignKey(
                name: "FK_CanBo_GiaDinhThuocDien_GiaDinhThuocDienMaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo",
                column: "GiaDinhThuocDienMaGiaDinhThuocDien",
                principalSchema: "tMasterData",
                principalTable: "GiaDinhThuocDien",
                principalColumn: "MaGiaDinhThuocDien");
        }
    }
}
