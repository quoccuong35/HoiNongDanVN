using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portal.DataAccess.Migrations
{
    public partial class addKyLuat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 7, 19, 58, 58, 581, DateTimeKind.Local).AddTicks(570),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 7, 19, 45, 41, 282, DateTimeKind.Local).AddTicks(7175));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 7, 19, 58, 58, 580, DateTimeKind.Local).AddTicks(6240),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 7, 19, 45, 41, 282, DateTimeKind.Local).AddTicks(3371));

            migrationBuilder.CreateTable(
                name: "QuaTrinhKyLuat",
                schema: "NS",
                columns: table => new
                {
                    IdQuaTrinhKyLuat = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDCanBo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SoQuyetDinh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NguoiKy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NgayKy = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LyDo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(550)", maxLength: 550, nullable: true),
                    MaHinhThucKyLuat = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuaTrinhKyLuat", x => x.IdQuaTrinhKyLuat);
                    table.ForeignKey(
                        name: "FK_QuaTrinhKyLuat_CanBo",
                        column: x => x.IDCanBo,
                        principalSchema: "NS",
                        principalTable: "CanBo",
                        principalColumn: "IDCanBo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuaTrinhKyLuat_HinhThucKyLuat",
                        column: x => x.MaHinhThucKyLuat,
                        principalSchema: "tMasterData",
                        principalTable: "HinhThucKyLuat",
                        principalColumn: "MaHinhThucKyLuat",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuaTrinhKyLuat_IDCanBo",
                schema: "NS",
                table: "QuaTrinhKyLuat",
                column: "IDCanBo");

            migrationBuilder.CreateIndex(
                name: "IX_QuaTrinhKyLuat_MaHinhThucKyLuat",
                schema: "NS",
                table: "QuaTrinhKyLuat",
                column: "MaHinhThucKyLuat");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuaTrinhKyLuat",
                schema: "NS");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 7, 19, 45, 41, 282, DateTimeKind.Local).AddTicks(7175),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 7, 19, 58, 58, 581, DateTimeKind.Local).AddTicks(570));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 7, 19, 45, 41, 282, DateTimeKind.Local).AddTicks(3371),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 7, 19, 58, 58, 580, DateTimeKind.Local).AddTicks(6240));
        }
    }
}
