using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class addHuuTri : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 6, 14, 19, 38, 9, 586, DateTimeKind.Local).AddTicks(3456),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 6, 13, 19, 55, 13, 339, DateTimeKind.Local).AddTicks(5199));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 6, 14, 19, 38, 9, 585, DateTimeKind.Local).AddTicks(9456),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 6, 13, 19, 55, 13, 339, DateTimeKind.Local).AddTicks(1373));

            migrationBuilder.CreateTable(
                name: "HuuTri",
                schema: "NS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDCanBo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NgayQuyetDinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoQuyetDinh = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NguoiKy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Actived = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HuuTri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HuuTri_CanBo",
                        column: x => x.IDCanBo,
                        principalSchema: "NS",
                        principalTable: "CanBo",
                        principalColumn: "IDCanBo");
                });

            migrationBuilder.CreateIndex(
                name: "IX_HuuTri_IDCanBo",
                schema: "NS",
                table: "HuuTri",
                column: "IDCanBo",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HuuTri",
                schema: "NS");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 6, 13, 19, 55, 13, 339, DateTimeKind.Local).AddTicks(5199),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 6, 14, 19, 38, 9, 586, DateTimeKind.Local).AddTicks(3456));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 6, 13, 19, 55, 13, 339, DateTimeKind.Local).AddTicks(1373),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 6, 14, 19, 38, 9, 585, DateTimeKind.Local).AddTicks(9456));
        }
    }
}
