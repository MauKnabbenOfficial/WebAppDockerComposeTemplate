using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebAppDockerTeste.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Batmans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Batmans", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Batmans",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("0f2e7396-5cef-4541-aaf9-95b92c789122"), "Batman2" },
                    { new Guid("74226486-7f03-4c66-8473-6453e6f32a8b"), "Batman3" },
                    { new Guid("b970630d-91ef-4881-a862-c6a5ef5807d5"), "Batman1" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Batmans");
        }
    }
}
