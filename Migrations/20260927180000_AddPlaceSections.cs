using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Taply.Models;

#nullable disable

namespace Taply.Migrations;

[DbContext(typeof(TaplyDbContext))]
[Migration("20260927180000_AddPlaceSections")]
public partial class AddPlaceSections : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "PlaceSections",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                PlaceId = table.Column<int>(type: "int", nullable: false),
                Type = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                Title = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                Subtitle = table.Column<string>(type: "nvarchar(240)", maxLength: 240, nullable: true),
                Icon = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ButtonText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ButtonUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SortOrder = table.Column<int>(type: "int", nullable: false),
                IsVisible = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PlaceSections", x => x.Id);
                table.ForeignKey(
                    name: "FK_PlaceSections_Places_PlaceId",
                    column: x => x.PlaceId,
                    principalTable: "Places",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_PlaceSections_PlaceId",
            table: "PlaceSections",
            column: "PlaceId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
        => migrationBuilder.DropTable(name: "PlaceSections");
}
