using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portal.DataAccess.Migrations
{
    public partial class addPhuongXa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 22, 13, 46, 24, 89, DateTimeKind.Local).AddTicks(1432),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 22, 13, 35, 16, 51, DateTimeKind.Local).AddTicks(1140));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 22, 13, 46, 24, 88, DateTimeKind.Local).AddTicks(7490),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 22, 13, 35, 16, 50, DateTimeKind.Local).AddTicks(7969));

            migrationBuilder.CreateTable(
                name: "PhuongXaModel",
                schema: "tMasterData",
                columns: table => new
                {
                    MaPhuongXa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenPhuongXa = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    MaQuanHuyen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Actived = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhuongXaModel", x => x.MaPhuongXa);
                    table.ForeignKey(
                        name: "FK_PhuongXa_QuanHuyen",
                        column: x => x.MaQuanHuyen,
                        principalSchema: "tMasterData",
                        principalTable: "QuanHuyenModel",
                        principalColumn: "MaQuanHuyen",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhuongXaModel_MaQuanHuyen",
                schema: "tMasterData",
                table: "PhuongXaModel",
                column: "MaQuanHuyen");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhuongXaModel",
                schema: "tMasterData");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 22, 13, 35, 16, 51, DateTimeKind.Local).AddTicks(1140),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 22, 13, 46, 24, 89, DateTimeKind.Local).AddTicks(1432));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 22, 13, 35, 16, 50, DateTimeKind.Local).AddTicks(7969),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 22, 13, 46, 24, 88, DateTimeKind.Local).AddTicks(7490));
        }
    }
}
