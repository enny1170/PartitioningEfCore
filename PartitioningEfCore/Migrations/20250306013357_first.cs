using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PartitioningEfCore.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
