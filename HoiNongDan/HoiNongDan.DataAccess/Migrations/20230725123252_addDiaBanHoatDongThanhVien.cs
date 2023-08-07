using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class addDiaBanHoatDongThanhVien : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 25, 19, 32, 50, 210, DateTimeKind.Local).AddTicks(8205),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 18, 11, 19, 5, 947, DateTimeKind.Local).AddTicks(7062));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 25, 19, 32, 50, 210, DateTimeKind.Local).AddTicks(3442),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 18, 11, 19, 5, 947, DateTimeKind.Local).AddTicks(3257));

            migrationBuilder.CreateTable(
                name: "DiaBanHoatDong_ThanhVien",
                schema: "HV",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdDiaBan = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDCanBo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaChucVu = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NgayVao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayRoiDi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LyDo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RoiDi = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaBanHoatDong_ThanhVien", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiaBanHoatDong_ThanhVien_CanBo",
                        column: x => x.IDCanBo,
                        principalSchema: "NS",
                        principalTable: "CanBo",
                        principalColumn: "IDCanBo");
                    table.ForeignKey(
                        name: "FK_DiaBanHoatDong_ThanhVien_ChucVu",
                        column: x => x.MaChucVu,
                        principalSchema: "tMasterData",
                        principalTable: "ChucVuModel",
                        principalColumn: "MaChucVu");
                    table.ForeignKey(
                        name: "FK_DiaBanHoatDong_ThanhVien_DiaBanHoatDong",
                        column: x => x.IdDiaBan,
                        principalSchema: "HV",
                        principalTable: "DiaBanHoatDong",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiaBanHoatDong_ThanhVien_IDCanBo",
                schema: "HV",
                table: "DiaBanHoatDong_ThanhVien",
                column: "IDCanBo");

            migrationBuilder.CreateIndex(
                name: "IX_DiaBanHoatDong_ThanhVien_IdDiaBan",
                schema: "HV",
                table: "DiaBanHoatDong_ThanhVien",
                column: "IdDiaBan");

            migrationBuilder.CreateIndex(
                name: "IX_DiaBanHoatDong_ThanhVien_MaChucVu",
                schema: "HV",
                table: "DiaBanHoatDong_ThanhVien",
                column: "MaChucVu");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiaBanHoatDong_ThanhVien",
                schema: "HV");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 18, 11, 19, 5, 947, DateTimeKind.Local).AddTicks(7062),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 25, 19, 32, 50, 210, DateTimeKind.Local).AddTicks(8205));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 18, 11, 19, 5, 947, DateTimeKind.Local).AddTicks(3257),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 25, 19, 32, 50, 210, DateTimeKind.Local).AddTicks(3442));
        }
    }
}
