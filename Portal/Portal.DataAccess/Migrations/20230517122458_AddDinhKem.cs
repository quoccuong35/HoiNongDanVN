using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portal.DataAccess.Migrations
{
    public partial class AddDinhKem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 17, 19, 24, 57, 122, DateTimeKind.Local).AddTicks(4238),
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
                defaultValue: new DateTime(2023, 5, 17, 19, 24, 57, 122, DateTimeKind.Local).AddTicks(1344),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 16, 18, 56, 26, 414, DateTimeKind.Local).AddTicks(9243));

            migrationBuilder.CreateTable(
                name: "LoaiDinhKem",
                schema: "tMasterData",
                columns: table => new
                {
                    IDLoaiDinhKem = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenLoaiDinhKem = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiDinhKem", x => x.IDLoaiDinhKem);
                });

            migrationBuilder.CreateTable(
                name: "FileDinhKem",
                schema: "tMasterData",
                columns: table => new
                {
                    Key = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDLoaiDinhKem = table.Column<string>(type: "nvarchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileDinhKem", x => x.Key);
                    table.ForeignKey(
                        name: "FK_FileDinhKem_LoaiDinhKem",
                        column: x => x.IDLoaiDinhKem,
                        principalSchema: "tMasterData",
                        principalTable: "LoaiDinhKem",
                        principalColumn: "IDLoaiDinhKem");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileDinhKem_IDLoaiDinhKem",
                schema: "tMasterData",
                table: "FileDinhKem",
                column: "IDLoaiDinhKem");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileDinhKem",
                schema: "tMasterData");

            migrationBuilder.DropTable(
                name: "LoaiDinhKem",
                schema: "tMasterData");

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
                oldDefaultValue: new DateTime(2023, 5, 17, 19, 24, 57, 122, DateTimeKind.Local).AddTicks(4238));

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
                oldDefaultValue: new DateTime(2023, 5, 17, 19, 24, 57, 122, DateTimeKind.Local).AddTicks(1344));
        }
    }
}
