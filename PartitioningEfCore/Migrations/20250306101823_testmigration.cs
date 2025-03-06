using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MigrationsBuilder;

#nullable disable

namespace PartitioningEfCore.Migrations
{
    /// <inheritdoc />
    public partial class testmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreatePartitionFunction("TestMertFunc",new List<short>(){2022,2023,2024});
            migrationBuilder.CreatePartitionScheme("TestMertSheme","TestMertFunc");
            migrationBuilder.CreateTable(
                name: "PartTestTable",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    year = table.Column<short>(type: "smallint", nullable: false),
                    test = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Test2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Test3 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PartTest__8A1A4A07B5F1E9F5", x => new { x.id, x.year });
                });

            migrationBuilder.CreateTable(
                name: "TestTable",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    year = table.Column<short>(type: "smallint", nullable: false),
                    test = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Test2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Test3 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TestTabl__8A1A4A07B536F814", x => new { x.id, x.year });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartTestTable");

            migrationBuilder.DropTable(
                name: "TestTable");
            migrationBuilder.DropPartitionScheme("TestMertSheme");
            migrationBuilder.DropPartitionFunction("TestMertFunc");
        }
    }
}
