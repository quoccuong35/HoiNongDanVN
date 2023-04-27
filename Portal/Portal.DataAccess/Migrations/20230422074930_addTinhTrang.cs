using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portal.DataAccess.Migrations
{
    public partial class addTinhTrang : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 22, 14, 49, 29, 772, DateTimeKind.Local).AddTicks(2788),
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
                defaultValue: new DateTime(2023, 4, 22, 14, 49, 29, 771, DateTimeKind.Local).AddTicks(8389),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 22, 13, 46, 24, 88, DateTimeKind.Local).AddTicks(7490));

            migrationBuilder.CreateTable(
                name: "HocHamModel",
                schema: "tMasterData",
                columns: table => new
                {
                    MaHocHam = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenHocHam = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HocHamModel", x => x.MaHocHam);
                });

            migrationBuilder.CreateTable(
                name: "TinhTrangModel",
                schema: "tMasterData",
                columns: table => new
                {
                    MaTinhTrang = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenTinhTrang = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
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
                    table.PrimaryKey("PK_TinhTrangModel", x => x.MaTinhTrang);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HocHamModel",
                schema: "tMasterData");

            migrationBuilder.DropTable(
                name: "TinhTrangModel",
                schema: "tMasterData");

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
                oldDefaultValue: new DateTime(2023, 4, 22, 14, 49, 29, 772, DateTimeKind.Local).AddTicks(2788));

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
                oldDefaultValue: new DateTime(2023, 4, 22, 14, 49, 29, 771, DateTimeKind.Local).AddTicks(8389));
        }
    }
}
