using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editCanBoHocVi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 18, 13, 34, 12, 444, DateTimeKind.Local).AddTicks(9862),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 17, 8, 39, 3, 178, DateTimeKind.Local).AddTicks(3021));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 18, 13, 34, 12, 444, DateTimeKind.Local).AddTicks(6324),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 17, 8, 39, 3, 178, DateTimeKind.Local).AddTicks(841));

            migrationBuilder.AddColumn<string>(
                name: "MaDinhDanh",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaHocVi",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HocViModel",
                schema: "tMasterData",
                columns: table => new
                {
                    MaHocVi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenHocVi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HocViModel", x => x.MaHocVi);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_MaHocVi",
                schema: "NS",
                table: "CanBo",
                column: "MaHocVi");

            migrationBuilder.AddForeignKey(
                name: "FK_CanBo_HocVi",
                schema: "NS",
                table: "CanBo",
                column: "MaHocVi",
                principalSchema: "tMasterData",
                principalTable: "HocViModel",
                principalColumn: "MaHocVi");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_HocVi",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropTable(
                name: "HocViModel",
                schema: "tMasterData");

            migrationBuilder.DropIndex(
                name: "IX_CanBo_MaHocVi",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "MaDinhDanh",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "MaHocVi",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 17, 8, 39, 3, 178, DateTimeKind.Local).AddTicks(3021),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 18, 13, 34, 12, 444, DateTimeKind.Local).AddTicks(9862));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 17, 8, 39, 3, 178, DateTimeKind.Local).AddTicks(841),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 18, 13, 34, 12, 444, DateTimeKind.Local).AddTicks(6324));
        }
    }
}
