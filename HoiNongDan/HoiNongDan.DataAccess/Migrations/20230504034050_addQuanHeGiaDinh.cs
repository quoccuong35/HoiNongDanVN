using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class addQuanHeGiaDinh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 4, 10, 40, 49, 457, DateTimeKind.Local).AddTicks(3019),
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
                defaultValue: new DateTime(2023, 5, 4, 10, 40, 49, 456, DateTimeKind.Local).AddTicks(8669),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 3, 16, 28, 32, 492, DateTimeKind.Local).AddTicks(7306));

            migrationBuilder.CreateTable(
                name: "LoaiQuanHeGiaDinh",
                schema: "tMasterData",
                columns: table => new
                {
                    IDLoaiQuanHeGiaDinh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenLoaiQuanHeGiaDinh = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Actived = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiQuanHeGiaDinh", x => x.IDLoaiQuanHeGiaDinh);
                });

            migrationBuilder.CreateTable(
                name: "QuanHeGiaDinh",
                schema: "NS",
                columns: table => new
                {
                    IDQuanheGiaDinh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDCanBo = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IDHoiVien = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HoTen = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NgaySinh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    NgheNghiep = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    NoiLamVien = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IDLoaiQuanHeGiaDinh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuanHeGiaDinh", x => x.IDQuanheGiaDinh);
                    table.ForeignKey(
                        name: "FK_QuanHeGiaDinh_CanBo",
                        column: x => x.IDCanBo,
                        principalSchema: "NS",
                        principalTable: "CanBo",
                        principalColumn: "IDCanBo");
                    table.ForeignKey(
                        name: "FK_QuanHeGiaDinh_LoaiQuanHeGiaDinh",
                        column: x => x.IDLoaiQuanHeGiaDinh,
                        principalSchema: "tMasterData",
                        principalTable: "LoaiQuanHeGiaDinh",
                        principalColumn: "IDLoaiQuanHeGiaDinh",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuanHeGiaDinh_IDCanBo",
                schema: "NS",
                table: "QuanHeGiaDinh",
                column: "IDCanBo");

            migrationBuilder.CreateIndex(
                name: "IX_QuanHeGiaDinh_IDLoaiQuanHeGiaDinh",
                schema: "NS",
                table: "QuanHeGiaDinh",
                column: "IDLoaiQuanHeGiaDinh");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuanHeGiaDinh",
                schema: "NS");

            migrationBuilder.DropTable(
                name: "LoaiQuanHeGiaDinh",
                schema: "tMasterData");

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
                oldDefaultValue: new DateTime(2023, 5, 4, 10, 40, 49, 457, DateTimeKind.Local).AddTicks(3019));

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
                oldDefaultValue: new DateTime(2023, 5, 4, 10, 40, 49, 456, DateTimeKind.Local).AddTicks(8669));
        }
    }
}
