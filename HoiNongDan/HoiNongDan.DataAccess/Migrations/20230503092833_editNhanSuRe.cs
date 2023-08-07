using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editNhanSuRe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 3, 16, 28, 32, 493, DateTimeKind.Local).AddTicks(1573),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 25, 21, 17, 10, 706, DateTimeKind.Local).AddTicks(6778));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 3, 16, 28, 32, 492, DateTimeKind.Local).AddTicks(7306),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 25, 21, 17, 10, 706, DateTimeKind.Local).AddTicks(2646));

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_MaHeDaoTao",
                schema: "NS",
                table: "CanBo",
                column: "MaHeDaoTao");

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_MaHocHam",
                schema: "NS",
                table: "CanBo",
                column: "MaHocHam");

            migrationBuilder.AddForeignKey(
                name: "FK_CanBo_HeDaoTao",
                schema: "NS",
                table: "CanBo",
                column: "MaHeDaoTao",
                principalSchema: "tMasterData",
                principalTable: "HeDaoTaoModel",
                principalColumn: "MaHeDaoTao",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CanBo_HocHam",
                schema: "NS",
                table: "CanBo",
                column: "MaHocHam",
                principalSchema: "tMasterData",
                principalTable: "HocHamModel",
                principalColumn: "MaHocHam");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_HeDaoTao",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_HocHam",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropIndex(
                name: "IX_CanBo_MaHeDaoTao",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropIndex(
                name: "IX_CanBo_MaHocHam",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 25, 21, 17, 10, 706, DateTimeKind.Local).AddTicks(6778),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 3, 16, 28, 32, 493, DateTimeKind.Local).AddTicks(1573));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 25, 21, 17, 10, 706, DateTimeKind.Local).AddTicks(2646),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 3, 16, 28, 32, 492, DateTimeKind.Local).AddTicks(7306));
        }
    }
}
