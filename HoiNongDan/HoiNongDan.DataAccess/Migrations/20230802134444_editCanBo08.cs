using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editCanBo08 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_HeDaoTao",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 2, 20, 44, 43, 230, DateTimeKind.Local).AddTicks(1784),
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
                defaultValue: new DateTime(2023, 8, 2, 20, 44, 43, 229, DateTimeKind.Local).AddTicks(6938),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 2, 19, 57, 41, 174, DateTimeKind.Local).AddTicks(2750));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "tMasterData",
                table: "ChucVuModel",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "MaHeDaoTao",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddForeignKey(
                name: "FK_CanBo_HeDaoTao",
                schema: "NS",
                table: "CanBo",
                column: "MaHeDaoTao",
                principalSchema: "tMasterData",
                principalTable: "HeDaoTaoModel",
                principalColumn: "MaHeDaoTao");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_HeDaoTao",
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
                oldDefaultValue: new DateTime(2023, 8, 2, 20, 44, 43, 230, DateTimeKind.Local).AddTicks(1784));

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
                oldDefaultValue: new DateTime(2023, 8, 2, 20, 44, 43, 229, DateTimeKind.Local).AddTicks(6938));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "tMasterData",
                table: "ChucVuModel",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaHeDaoTao",
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
                name: "FK_CanBo_HeDaoTao",
                schema: "NS",
                table: "CanBo",
                column: "MaHeDaoTao",
                principalSchema: "tMasterData",
                principalTable: "HeDaoTaoModel",
                principalColumn: "MaHeDaoTao",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
