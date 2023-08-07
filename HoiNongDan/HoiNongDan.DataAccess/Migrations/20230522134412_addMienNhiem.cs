using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class addMienNhiem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 22, 20, 44, 10, 659, DateTimeKind.Local).AddTicks(5257),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 21, 9, 14, 33, 709, DateTimeKind.Local).AddTicks(5189));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 22, 20, 44, 10, 659, DateTimeKind.Local).AddTicks(811),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 21, 9, 14, 33, 709, DateTimeKind.Local).AddTicks(304));

            migrationBuilder.CreateTable(
                name: "QuaTrinhMienNhiem",
                schema: "NS",
                columns: table => new
                {
                    IDQuaTrinhMienNhiem = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDCanBo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SoQuyetDinh = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NgayQuyetDinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HeSoChucVu = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NguoiKy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IdCoSo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdDepartment = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaChucVu = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuaTrinhMienNhiem", x => x.IDQuaTrinhMienNhiem);
                    table.ForeignKey(
                        name: "FK_QuaTrinhMienNhiem_CanBo",
                        column: x => x.IDCanBo,
                        principalSchema: "NS",
                        principalTable: "CanBo",
                        principalColumn: "IDCanBo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuaTrinhMienNhiem_ChucVu",
                        column: x => x.MaChucVu,
                        principalSchema: "tMasterData",
                        principalTable: "ChucVuModel",
                        principalColumn: "MaChucVu");
                    table.ForeignKey(
                        name: "FK_QuaTrinhMienNhiem_CoSo",
                        column: x => x.IdCoSo,
                        principalSchema: "tMasterData",
                        principalTable: "CoSoModel",
                        principalColumn: "IdCoSo");
                    table.ForeignKey(
                        name: "FK_QuaTrinhMienNhiem_Department",
                        column: x => x.IdDepartment,
                        principalSchema: "tMasterData",
                        principalTable: "Department",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuaTrinhMienNhiem_IDCanBo",
                schema: "NS",
                table: "QuaTrinhMienNhiem",
                column: "IDCanBo");

            migrationBuilder.CreateIndex(
                name: "IX_QuaTrinhMienNhiem_IdCoSo",
                schema: "NS",
                table: "QuaTrinhMienNhiem",
                column: "IdCoSo");

            migrationBuilder.CreateIndex(
                name: "IX_QuaTrinhMienNhiem_IdDepartment",
                schema: "NS",
                table: "QuaTrinhMienNhiem",
                column: "IdDepartment");

            migrationBuilder.CreateIndex(
                name: "IX_QuaTrinhMienNhiem_MaChucVu",
                schema: "NS",
                table: "QuaTrinhMienNhiem",
                column: "MaChucVu");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuaTrinhMienNhiem",
                schema: "NS");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 21, 9, 14, 33, 709, DateTimeKind.Local).AddTicks(5189),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 22, 20, 44, 10, 659, DateTimeKind.Local).AddTicks(5257));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 21, 9, 14, 33, 709, DateTimeKind.Local).AddTicks(304),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 22, 20, 44, 10, 659, DateTimeKind.Local).AddTicks(811));
        }
    }
}
