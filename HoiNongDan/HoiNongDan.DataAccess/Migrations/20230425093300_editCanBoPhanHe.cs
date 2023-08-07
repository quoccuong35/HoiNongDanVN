using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editCanBoPhanHe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 25, 16, 32, 59, 222, DateTimeKind.Local).AddTicks(6931),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 25, 11, 22, 16, 704, DateTimeKind.Local).AddTicks(2028));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 25, 16, 32, 59, 222, DateTimeKind.Local).AddTicks(2210),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 25, 11, 22, 16, 703, DateTimeKind.Local).AddTicks(7278));

            migrationBuilder.AddColumn<string>(
                name: "MaPhanHe",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_MaPhanHe",
                schema: "NS",
                table: "CanBo",
                column: "MaPhanHe");

            migrationBuilder.AddForeignKey(
                name: "FK_CanBo_PhanHe",
                schema: "NS",
                table: "CanBo",
                column: "MaPhanHe",
                principalSchema: "tMasterData",
                principalTable: "PhanHeModel",
                principalColumn: "MaPhanHe",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_PhanHe",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropIndex(
                name: "IX_CanBo_MaPhanHe",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "MaPhanHe",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 25, 11, 22, 16, 704, DateTimeKind.Local).AddTicks(2028),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 25, 16, 32, 59, 222, DateTimeKind.Local).AddTicks(6931));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 25, 11, 22, 16, 703, DateTimeKind.Local).AddTicks(7278),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 25, 16, 32, 59, 222, DateTimeKind.Local).AddTicks(2210));
        }
    }
}
