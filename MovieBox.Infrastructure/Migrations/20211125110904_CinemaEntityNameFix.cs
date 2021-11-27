using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieBox.Infrastructure.Migrations
{
    public partial class CinemaEntityNameFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieCinemasMovies_MovieCinemas_MovieTheaterId",
                table: "MovieCinemasMovies");

            migrationBuilder.RenameColumn(
                name: "InTheaters",
                table: "Movies",
                newName: "InCinemas");

            migrationBuilder.RenameColumn(
                name: "MovieTheaterId",
                table: "MovieCinemasMovies",
                newName: "MovieCinemaId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCinemasMovies_MovieCinemas_MovieCinemaId",
                table: "MovieCinemasMovies",
                column: "MovieCinemaId",
                principalTable: "MovieCinemas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieCinemasMovies_MovieCinemas_MovieCinemaId",
                table: "MovieCinemasMovies");

            migrationBuilder.RenameColumn(
                name: "InCinemas",
                table: "Movies",
                newName: "InTheaters");

            migrationBuilder.RenameColumn(
                name: "MovieCinemaId",
                table: "MovieCinemasMovies",
                newName: "MovieTheaterId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCinemasMovies_MovieCinemas_MovieTheaterId",
                table: "MovieCinemasMovies",
                column: "MovieTheaterId",
                principalTable: "MovieCinemas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
