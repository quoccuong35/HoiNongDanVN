using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class addBoNhiem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 16, 18, 56, 26, 415, DateTimeKind.Local).AddTicks(3044),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 13, 9, 17, 8, 687, DateTimeKind.Local).AddTicks(5177));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 16, 18, 56, 26, 414, DateTimeKind.Local).AddTicks(9243),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 13, 9, 17, 8, 687, DateTimeKind.Local).AddTicks(1073));

            migrationBuilder.CreateTable(
                name: "QuaTrinhBoNhiem",
                schema: "NS",
                columns: table => new
                {
                    IdQuaTrinhBoNhiem = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDCanBo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NgayQuyetDinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoQuyetDinh = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NguoiKy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IdCoSo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdDepartment = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaChucVu = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HeSoChucVu = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FileDinhKem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuaTrinhBoNhiem", x => x.IdQuaTrinhBoNhiem);
                    table.ForeignKey(
                        name: "FK_QuaTrinhBoNhiem_CanBo",
                        column: x => x.IDCanBo,
                        principalSchema: "NS",
                        principalTable: "CanBo",
                        principalColumn: "IDCanBo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuaTrinhBoNhiem_ChucVu",
                        column: x => x.MaChucVu,
                        principalSchema: "tMasterData",
                        principalTable: "ChucVuModel",
                        principalColumn: "MaChucVu");
                    table.ForeignKey(
                        name: "FK_QuaTrinhBoNhiem_CoSo",
                        column: x => x.IdCoSo,
                        principalSchema: "tMasterData",
                        principalTable: "CoSoModel",
                        principalColumn: "IdCoSo");
                    table.ForeignKey(
                        name: "FK_QuaTrinhBoNhiem_Department",
                        column: x => x.IdDepartment,
                        principalSchema: "tMasterData",
                        principalTable: "Department",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuaTrinhBoNhiem_IDCanBo",
                schema: "NS",
                table: "QuaTrinhBoNhiem",
                column: "IDCanBo");

            migrationBuilder.CreateIndex(
                name: "IX_QuaTrinhBoNhiem_IdCoSo",
                schema: "NS",
                table: "QuaTrinhBoNhiem",
                column: "IdCoSo");

            migrationBuilder.CreateIndex(
                name: "IX_QuaTrinhBoNhiem_IdDepartment",
                schema: "NS",
                table: "QuaTrinhBoNhiem",
                column: "IdDepartment");

            migrationBuilder.CreateIndex(
                name: "IX_QuaTrinhBoNhiem_MaChucVu",
                schema: "NS",
                table: "QuaTrinhBoNhiem",
                column: "MaChucVu");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuaTrinhBoNhiem",
                schema: "NS");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 13, 9, 17, 8, 687, DateTimeKind.Local).AddTicks(5177),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 16, 18, 56, 26, 415, DateTimeKind.Local).AddTicks(3044));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 13, 9, 17, 8, 687, DateTimeKind.Local).AddTicks(1073),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 16, 18, 56, 26, 414, DateTimeKind.Local).AddTicks(9243));
        }
    }
}
