using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class EditCanBoXoaChuyenNganh1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_TrinhDoChuyenMon_TrinhDoChuyenMonMaTrinhDoChuyenMon",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropIndex(
                name: "IX_CanBo_TrinhDoChuyenMonMaTrinhDoChuyenMon",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "TrinhDoChuyenMonMaTrinhDoChuyenMon",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 12, 15, 12, 49, 562, DateTimeKind.Local).AddTicks(4969),
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
                defaultValue: new DateTime(2023, 8, 12, 15, 12, 49, 562, DateTimeKind.Local).AddTicks(310),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 12, 15, 10, 41, 551, DateTimeKind.Local).AddTicks(1003));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                oldDefaultValue: new DateTime(2023, 8, 12, 15, 12, 49, 562, DateTimeKind.Local).AddTicks(4969));

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
                oldDefaultValue: new DateTime(2023, 8, 12, 15, 12, 49, 562, DateTimeKind.Local).AddTicks(310));

            migrationBuilder.AddColumn<Guid>(
                name: "TrinhDoChuyenMonMaTrinhDoChuyenMon",
                schema: "NS",
                table: "CanBo",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_TrinhDoChuyenMonMaTrinhDoChuyenMon",
                schema: "NS",
                table: "CanBo",
                column: "TrinhDoChuyenMonMaTrinhDoChuyenMon");

            migrationBuilder.AddForeignKey(
                name: "FK_CanBo_TrinhDoChuyenMon_TrinhDoChuyenMonMaTrinhDoChuyenMon",
                schema: "NS",
                table: "CanBo",
                column: "TrinhDoChuyenMonMaTrinhDoChuyenMon",
                principalSchema: "tMasterData",
                principalTable: "TrinhDoChuyenMon",
                principalColumn: "MaTrinhDoChuyenMon");
        }
    }
}
