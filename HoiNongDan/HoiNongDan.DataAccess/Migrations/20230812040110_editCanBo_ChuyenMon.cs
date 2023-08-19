using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editCanBo_ChuyenMon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_TrinhDoHocVan",
                schema: "NS",
                table: "CanBo");

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
                oldDefaultValue: new DateTime(2023, 8, 2, 21, 13, 43, 788, DateTimeKind.Local).AddTicks(1762));

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
                oldDefaultValue: new DateTime(2023, 8, 2, 21, 13, 43, 787, DateTimeKind.Local).AddTicks(8149));

            migrationBuilder.AlterColumn<string>(
                name: "MaTrinhDoHocVan",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<Guid>(
                name: "MaTrinhDoChuyenMon",
                schema: "NS",
                table: "CanBo",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TrinhDoChuyenMon",
                schema: "tMasterData",
                columns: table => new
                {
                    MaTrinhDoChuyenMon = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenTrinhDoChuyenMon = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrinhDoChuyenMon", x => x.MaTrinhDoChuyenMon);
                });

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

            migrationBuilder.AddForeignKey(
                name: "FK_CanBo_TrinhDoHocVan",
                schema: "NS",
                table: "CanBo",
                column: "MaTrinhDoHocVan",
                principalSchema: "tMasterData",
                principalTable: "TrinhDoHocVanModel",
                principalColumn: "MaTrinhDoHocVan");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_TrinhDoChuyenMon",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_TrinhDoHocVan",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropTable(
                name: "TrinhDoChuyenMon",
                schema: "tMasterData");

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
                defaultValue: new DateTime(2023, 8, 2, 21, 13, 43, 788, DateTimeKind.Local).AddTicks(1762),
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
                defaultValue: new DateTime(2023, 8, 2, 21, 13, 43, 787, DateTimeKind.Local).AddTicks(8149),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 12, 11, 1, 9, 665, DateTimeKind.Local).AddTicks(957));

            migrationBuilder.AlterColumn<string>(
                name: "MaTrinhDoHocVan",
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
                name: "FK_CanBo_TrinhDoHocVan",
                schema: "NS",
                table: "CanBo",
                column: "MaTrinhDoHocVan",
                principalSchema: "tMasterData",
                principalTable: "TrinhDoHocVanModel",
                principalColumn: "MaTrinhDoHocVan",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
