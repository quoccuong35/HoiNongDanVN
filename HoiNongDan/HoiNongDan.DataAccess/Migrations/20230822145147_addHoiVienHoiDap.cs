using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class addHoiVienHoiDap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 22, 21, 51, 46, 170, DateTimeKind.Local).AddTicks(8822),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 21, 19, 1, 4, 659, DateTimeKind.Local).AddTicks(8838));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 22, 21, 51, 46, 170, DateTimeKind.Local).AddTicks(5884),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 21, 19, 1, 4, 659, DateTimeKind.Local).AddTicks(6665));

            migrationBuilder.CreateTable(
                name: "HoiVienHoiDap",
                schema: "HV",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDHoivien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IdParent = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoiVienHoiDap", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HoiVienHoiDap_CanBo",
                        column: x => x.IDHoivien,
                        principalSchema: "NS",
                        principalTable: "CanBo",
                        principalColumn: "IDCanBo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HoiVienHoiDap_IDHoivien",
                schema: "HV",
                table: "HoiVienHoiDap",
                column: "IDHoivien");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HoiVienHoiDap",
                schema: "HV");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 21, 19, 1, 4, 659, DateTimeKind.Local).AddTicks(8838),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 22, 21, 51, 46, 170, DateTimeKind.Local).AddTicks(8822));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 21, 19, 1, 4, 659, DateTimeKind.Local).AddTicks(6665),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 22, 21, 51, 46, 170, DateTimeKind.Local).AddTicks(5884));
        }
    }
}
