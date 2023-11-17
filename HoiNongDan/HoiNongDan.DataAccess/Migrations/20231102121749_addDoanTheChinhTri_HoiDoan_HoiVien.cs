using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class addDoanTheChinhTri_HoiDoan_HoiVien : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                oldDefaultValue: new DateTime(2023, 10, 29, 15, 58, 22, 740, DateTimeKind.Local).AddTicks(6030));

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
                oldDefaultValue: new DateTime(2023, 10, 29, 15, 58, 22, 740, DateTimeKind.Local).AddTicks(2455));

            migrationBuilder.CreateTable(
                name: "DoanTheChinhTri_HoiDoan_HoiVien",
                schema: "HV",
                columns: table => new
                {
                    IDHoiVien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaDoanTheChinhTri_HoiDoan = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NgayThamGia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayRoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LyDoRoi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoanTheChinhTri_HoiDoan_HoiVien", x => new { x.MaDoanTheChinhTri_HoiDoan, x.IDHoiVien });
                    table.ForeignKey(
                        name: "FK_DoanTheChinhTri_HoiDoan_HoiVien",
                        column: x => x.IDHoiVien,
                        principalSchema: "NS",
                        principalTable: "CanBo",
                        principalColumn: "IDCanBo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoanTheChinhTri_HoiDoan_HoiVien_MaDoanTheChinhTri_HoiDoan",
                        column: x => x.MaDoanTheChinhTri_HoiDoan,
                        principalSchema: "tMasterData",
                        principalTable: "DoanTheChinhTri_HoiDoan",
                        principalColumn: "MaDoanTheChinhTri_HoiDoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoanTheChinhTri_HoiDoan_HoiVien_IDHoiVien",
                schema: "HV",
                table: "DoanTheChinhTri_HoiDoan_HoiVien",
                column: "IDHoiVien");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoanTheChinhTri_HoiDoan_HoiVien",
                schema: "HV");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 10, 29, 15, 58, 22, 740, DateTimeKind.Local).AddTicks(6030),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 11, 2, 19, 17, 48, 989, DateTimeKind.Local).AddTicks(626));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 10, 29, 15, 58, 22, 740, DateTimeKind.Local).AddTicks(2455),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 11, 2, 19, 17, 48, 988, DateTimeKind.Local).AddTicks(7764));
        }
    }
}
