using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieRestful.Repository.Migrations
{
    public partial class ViewedMovieCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ViewedMovieCount",
                table: "mytable",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ViewedMovieCount",
                table: "mytable");
        }
    }
}
