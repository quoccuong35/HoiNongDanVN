using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class addPhamVi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 11, 4, 15, 11, 39, 210, DateTimeKind.Local).AddTicks(7728),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 11, 2, 19, 17, 48, 989, DateTimeKind.Local).AddTicks(626));

            migrationBuilder.AlterColumn<string>(
                name: "LyDoRoi",
                schema: "HV",
                table: "DoanTheChinhTri_HoiDoan_HoiVien",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 11, 4, 15, 11, 39, 210, DateTimeKind.Local).AddTicks(5251),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 11, 2, 19, 17, 48, 988, DateTimeKind.Local).AddTicks(7764));

            migrationBuilder.CreateTable(
                name: "PhamVi",
                schema: "pms",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaDiabanHoatDong = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhamVi", x => new { x.AccountId, x.MaDiabanHoatDong });
                    table.ForeignKey(
                        name: "FK_PhamVi_Account",
                        column: x => x.AccountId,
                        principalSchema: "pms",
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhamVi_DiaBanHoatDong",
                        column: x => x.MaDiabanHoatDong,
                        principalSchema: "HV",
                        principalTable: "DiaBanHoatDong",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhamVi_MaDiabanHoatDong",
                schema: "pms",
                table: "PhamVi",
                column: "MaDiabanHoatDong");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhamVi",
                schema: "pms");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 11, 2, 19, 17, 48, 989, DateTimeKind.Local).AddTicks(626),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 11, 4, 15, 11, 39, 210, DateTimeKind.Local).AddTicks(7728));

            migrationBuilder.AlterColumn<string>(
                name: "LyDoRoi",
                schema: "HV",
                table: "DoanTheChinhTri_HoiDoan_HoiVien",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 11, 2, 19, 17, 48, 988, DateTimeKind.Local).AddTicks(7764),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 11, 4, 15, 11, 39, 210, DateTimeKind.Local).AddTicks(5251));
        }
    }
}
