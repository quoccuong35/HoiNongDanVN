using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editCanBo07 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_PhanHe",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropForeignKey(
                name: "FKg_CanBo_TinhTrang",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 2, 19, 57, 41, 174, DateTimeKind.Local).AddTicks(6881),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 1, 21, 21, 46, 196, DateTimeKind.Local).AddTicks(8082));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 2, 19, 57, 41, 174, DateTimeKind.Local).AddTicks(2750),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 1, 21, 21, 46, 196, DateTimeKind.Local).AddTicks(3424));

            migrationBuilder.AlterColumn<string>(
                name: "MaTinhTrang",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "MaPhanHe",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddForeignKey(
                name: "FK_CanBo_PhanHe",
                schema: "NS",
                table: "CanBo",
                column: "MaPhanHe",
                principalSchema: "tMasterData",
                principalTable: "PhanHeModel",
                principalColumn: "MaPhanHe");

            migrationBuilder.AddForeignKey(
                name: "FKg_CanBo_TinhTrang",
                schema: "NS",
                table: "CanBo",
                column: "MaTinhTrang",
                principalSchema: "tMasterData",
                principalTable: "TinhTrangModel",
                principalColumn: "MaTinhTrang");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_PhanHe",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropForeignKey(
                name: "FKg_CanBo_TinhTrang",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 1, 21, 21, 46, 196, DateTimeKind.Local).AddTicks(8082),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 2, 19, 57, 41, 174, DateTimeKind.Local).AddTicks(6881));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 1, 21, 21, 46, 196, DateTimeKind.Local).AddTicks(3424),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 2, 19, 57, 41, 174, DateTimeKind.Local).AddTicks(2750));

            migrationBuilder.AlterColumn<string>(
                name: "MaTinhTrang",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaPhanHe",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CanBo_PhanHe",
                schema: "NS",
                table: "CanBo",
                column: "MaPhanHe",
                principalSchema: "tMasterData",
                principalTable: "PhanHeModel",
                principalColumn: "MaPhanHe",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FKg_CanBo_TinhTrang",
                schema: "NS",
                table: "CanBo",
                column: "MaTinhTrang",
                principalSchema: "tMasterData",
                principalTable: "TinhTrangModel",
                principalColumn: "MaTinhTrang",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
