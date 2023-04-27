using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portal.DataAccess.Migrations
{
    public partial class addChucVu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 22, 11, 5, 54, 987, DateTimeKind.Local).AddTicks(9482),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 21, 20, 16, 18, 64, DateTimeKind.Local).AddTicks(6589));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 22, 11, 5, 54, 987, DateTimeKind.Local).AddTicks(6042),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 21, 20, 16, 18, 64, DateTimeKind.Local).AddTicks(2122));

            migrationBuilder.CreateTable(
                name: "ChucVuModel",
                schema: "tMasterData",
                columns: table => new
                {
                    MaChucVu = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenChucVu = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    HeSoChucVu = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PhuCapDienThoai = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Actived = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChucVuModel", x => x.MaChucVu);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChucVuModel",
                schema: "tMasterData");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 21, 20, 16, 18, 64, DateTimeKind.Local).AddTicks(6589),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 22, 11, 5, 54, 987, DateTimeKind.Local).AddTicks(9482));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 21, 20, 16, 18, 64, DateTimeKind.Local).AddTicks(2122),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 22, 11, 5, 54, 987, DateTimeKind.Local).AddTicks(6042));
        }
    }
}
