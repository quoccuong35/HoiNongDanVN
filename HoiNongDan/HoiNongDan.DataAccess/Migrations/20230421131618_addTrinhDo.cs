using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class addTrinhDo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectModel",
                schema: "entity");

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
                oldDefaultValue: new DateTime(2023, 4, 20, 20, 49, 29, 92, DateTimeKind.Local).AddTicks(1855));

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
                oldDefaultValue: new DateTime(2023, 4, 20, 20, 49, 29, 91, DateTimeKind.Local).AddTicks(8473));

            migrationBuilder.CreateTable(
                name: "NgonNguModel",
                schema: "tMasterData",
                columns: table => new
                {
                    MaNgonNgu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenNgonNgu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Actived = table.Column<bool>(type: "bit", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NgonNguModel", x => x.MaNgonNgu);
                });

            migrationBuilder.CreateTable(
                name: "TrinhDoChinhTriModel",
                schema: "tMasterData",
                columns: table => new
                {
                    MaTrinhDoChinhTri = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenTrinhDoChinhTri = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Actived = table.Column<bool>(type: "bit", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrinhDoChinhTriModel", x => x.MaTrinhDoChinhTri);
                });

            migrationBuilder.CreateTable(
                name: "TrinhDoHocVanModel",
                schema: "tMasterData",
                columns: table => new
                {
                    MaTrinhDoHocVan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenTrinhDoHocVan = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Actived = table.Column<bool>(type: "bit", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrinhDoHocVanModel", x => x.MaTrinhDoHocVan);
                });

            migrationBuilder.CreateTable(
                name: "TrinhDoTinHocModel",
                schema: "tMasterData",
                columns: table => new
                {
                    MaTrinhDoTinHoc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenTrinhDoTinHoc = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Actived = table.Column<bool>(type: "bit", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrinhDoTinHocModel", x => x.MaTrinhDoTinHoc);
                });

            migrationBuilder.CreateTable(
                name: "TrinhDoNgoaiNguModel",
                schema: "tMasterData",
                columns: table => new
                {
                    MaTrinhDoNgoaiNgu = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenTrinhDoNgoaiNgu = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Actived = table.Column<bool>(type: "bit", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaNgonNgu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrinhDoNgoaiNguModel", x => x.MaTrinhDoNgoaiNgu);
                    table.ForeignKey(
                        name: "FK_TrinhDoNgoaiNgu_TrinhDoNgoaiNgu",
                        column: x => x.MaNgonNgu,
                        principalSchema: "tMasterData",
                        principalTable: "NgonNguModel",
                        principalColumn: "MaNgonNgu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrinhDoNgoaiNguModel_MaNgonNgu",
                schema: "tMasterData",
                table: "TrinhDoNgoaiNguModel",
                column: "MaNgonNgu");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrinhDoChinhTriModel",
                schema: "tMasterData");

            migrationBuilder.DropTable(
                name: "TrinhDoHocVanModel",
                schema: "tMasterData");

            migrationBuilder.DropTable(
                name: "TrinhDoNgoaiNguModel",
                schema: "tMasterData");

            migrationBuilder.DropTable(
                name: "TrinhDoTinHocModel",
                schema: "tMasterData");

            migrationBuilder.DropTable(
                name: "NgonNguModel",
                schema: "tMasterData");

            migrationBuilder.EnsureSchema(
                name: "entity");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 20, 20, 49, 29, 92, DateTimeKind.Local).AddTicks(1855),
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
                defaultValue: new DateTime(2023, 4, 20, 20, 49, 29, 91, DateTimeKind.Local).AddTicks(8473),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 21, 20, 16, 18, 64, DateTimeKind.Local).AddTicks(2122));

            migrationBuilder.CreateTable(
                name: "ProjectModel",
                schema: "entity",
                columns: table => new
                {
                    ProjectID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Actived = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValue: new DateTime(2023, 4, 20, 20, 49, 29, 92, DateTimeKind.Local).AddTicks(5492)),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: true),
                    ProjectName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectModel", x => x.ProjectID);
                });
        }
    }
}
