using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class EditCanBoXoaChuyenNganh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_TrinhDoChuyenMon",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.RenameColumn(
                name: "MaTrinhDoChuyenMon",
                schema: "NS",
                table: "CanBo",
                newName: "TrinhDoChuyenMonMaTrinhDoChuyenMon");

            migrationBuilder.RenameIndex(
                name: "IX_CanBo_MaTrinhDoChuyenMon",
                schema: "NS",
                table: "CanBo",
                newName: "IX_CanBo_TrinhDoChuyenMonMaTrinhDoChuyenMon");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 12, 15, 10, 41, 551, DateTimeKind.Local).AddTicks(4737),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 12, 11, 1, 9, 665, DateTimeKind.Local).AddTicks(8746));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 12, 15, 10, 41, 551, DateTimeKind.Local).AddTicks(1003),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 12, 11, 1, 9, 665, DateTimeKind.Local).AddTicks(957));

            migrationBuilder.AddForeignKey(
                name: "FK_CanBo_TrinhDoChuyenMon_TrinhDoChuyenMonMaTrinhDoChuyenMon",
                schema: "NS",
                table: "CanBo",
                column: "TrinhDoChuyenMonMaTrinhDoChuyenMon",
                principalSchema: "tMasterData",
                principalTable: "TrinhDoChuyenMon",
                principalColumn: "MaTrinhDoChuyenMon");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_TrinhDoChuyenMon_TrinhDoChuyenMonMaTrinhDoChuyenMon",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.RenameColumn(
                name: "TrinhDoChuyenMonMaTrinhDoChuyenMon",
                schema: "NS",
                table: "CanBo",
                newName: "MaTrinhDoChuyenMon");

            migrationBuilder.RenameIndex(
                name: "IX_CanBo_TrinhDoChuyenMonMaTrinhDoChuyenMon",
                schema: "NS",
                table: "CanBo",
                newName: "IX_CanBo_MaTrinhDoChuyenMon");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 12, 11, 1, 9, 665, DateTimeKind.Local).AddTicks(8746),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 12, 15, 10, 41, 551, DateTimeKind.Local).AddTicks(4737));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 12, 11, 1, 9, 665, DateTimeKind.Local).AddTicks(957),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 12, 15, 10, 41, 551, DateTimeKind.Local).AddTicks(1003));

            migrationBuilder.AddForeignKey(
                name: "FK_CanBo_TrinhDoChuyenMon",
                schema: "NS",
                table: "CanBo",
                column: "MaTrinhDoChuyenMon",
                principalSchema: "tMasterData",
                principalTable: "TrinhDoChuyenMon",
                principalColumn: "MaTrinhDoChuyenMon");
        }
    }
}
