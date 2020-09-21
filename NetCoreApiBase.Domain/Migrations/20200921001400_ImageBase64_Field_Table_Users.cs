using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreApiBase.Domain.Migrations
{
    public partial class ImageBase64_Field_Table_Users : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageBaseData",
                table: "Users",
                type: "nvarchar(MAX)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageBaseData",
                table: "Users");
        }
    }
}
