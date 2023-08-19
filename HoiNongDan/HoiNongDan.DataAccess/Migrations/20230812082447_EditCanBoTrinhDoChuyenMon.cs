using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class EditCanBoTrinhDoChuyenMon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 12, 15, 24, 46, 626, DateTimeKind.Local).AddTicks(154),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 12, 15, 19, 43, 421, DateTimeKind.Local).AddTicks(5908));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 12, 15, 24, 46, 625, DateTimeKind.Local).AddTicks(5275),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 12, 15, 19, 43, 421, DateTimeKind.Local).AddTicks(1729));

            migrationBuilder.AddColumn<string>(
                name: "MaTrinhDoChuyenMon",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_MaTrinhDoChuyenMon",
                schema: "NS",
                table: "CanBo",
                column: "MaTrinhDoChuyenMon");

            migrationBuilder.AddForeignKey(
                name: "FK_CanBo_TrinhDoChuyenMon",
                schema: "NS",
                table: "CanBo",
                column: "MaTrinhDoChuyenMon",
                principalSchema: "tMasterData",
                principalTable: "TrinhDoChuyenMon",
                principalColumn: "MaTrinhDoChuyenMon");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_TrinhDoChuyenMon",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropIndex(
                name: "IX_CanBo_MaTrinhDoChuyenMon",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "MaTrinhDoChuyenMon",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 12, 15, 19, 43, 421, DateTimeKind.Local).AddTicks(5908),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 12, 15, 24, 46, 626, DateTimeKind.Local).AddTicks(154));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 12, 15, 19, 43, 421, DateTimeKind.Local).AddTicks(1729),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 12, 15, 24, 46, 625, DateTimeKind.Local).AddTicks(5275));
        }
    }
}
