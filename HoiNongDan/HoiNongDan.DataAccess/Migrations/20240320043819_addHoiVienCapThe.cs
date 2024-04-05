using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class addHoiVienCapThe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 3, 20, 11, 38, 19, 48, DateTimeKind.Local).AddTicks(7200),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 3, 20, 11, 4, 46, 789, DateTimeKind.Local).AddTicks(8438));

            migrationBuilder.AlterColumn<string>(
                name: "TenDot",
                schema: "tMasterData",
                table: "Dot",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 3, 20, 11, 38, 19, 48, DateTimeKind.Local).AddTicks(4512),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 3, 20, 11, 4, 46, 789, DateTimeKind.Local).AddTicks(5785));

            migrationBuilder.CreateTable(
                name: "HoiVienCapThe",
                schema: "HV",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaDot = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDHoiVien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nam = table.Column<int>(type: "int", nullable: false),
                    SoThe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayCap = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Actived = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LoaiCap = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoiVienCapThe", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HoiVienCapThe_CanBo",
                        column: x => x.IDHoiVien,
                        principalSchema: "NS",
                        principalTable: "CanBo",
                        principalColumn: "IDCanBo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoiVienCapThe_Dot",
                        column: x => x.MaDot,
                        principalSchema: "tMasterData",
                        principalTable: "Dot",
                        principalColumn: "MaDot",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HoiVienCapThe_IDHoiVien",
                schema: "HV",
                table: "HoiVienCapThe",
                column: "IDHoiVien");

            migrationBuilder.CreateIndex(
                name: "IX_HoiVienCapThe_MaDot",
                schema: "HV",
                table: "HoiVienCapThe",
                column: "MaDot");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HoiVienCapThe",
                schema: "HV");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 3, 20, 11, 4, 46, 789, DateTimeKind.Local).AddTicks(8438),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 3, 20, 11, 38, 19, 48, DateTimeKind.Local).AddTicks(7200));

            migrationBuilder.AlterColumn<string>(
                name: "TenDot",
                schema: "tMasterData",
                table: "Dot",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 3, 20, 11, 4, 46, 789, DateTimeKind.Local).AddTicks(5785),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 3, 20, 11, 38, 19, 48, DateTimeKind.Local).AddTicks(4512));
        }
    }
}
