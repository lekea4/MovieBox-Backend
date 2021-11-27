using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieBox.Infrastructure.Migrations
{
    public partial class FixedEntitiesNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieTheatersMovies_Movies_MovieId",
                table: "MovieTheatersMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieTheatersMovies_MovieTheaters_MovieTheaterId",
                table: "MovieTheatersMovies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieTheatersMovies",
                table: "MovieTheatersMovies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieTheaters",
                table: "MovieTheaters");

            migrationBuilder.RenameTable(
                name: "MovieTheatersMovies",
                newName: "MovieCinemasMovies");

            migrationBuilder.RenameTable(
                name: "MovieTheaters",
                newName: "MovieCinemas");

            migrationBuilder.RenameIndex(
                name: "IX_MovieTheatersMovies_MovieId",
                table: "MovieCinemasMovies",
                newName: "IX_MovieCinemasMovies_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieCinemasMovies",
                table: "MovieCinemasMovies",
                columns: new[] { "MovieTheaterId", "MovieId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieCinemas",
                table: "MovieCinemas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCinemasMovies_MovieCinemas_MovieTheaterId",
                table: "MovieCinemasMovies",
                column: "MovieTheaterId",
                principalTable: "MovieCinemas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCinemasMovies_Movies_MovieId",
                table: "MovieCinemasMovies",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieCinemasMovies_MovieCinemas_MovieTheaterId",
                table: "MovieCinemasMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieCinemasMovies_Movies_MovieId",
                table: "MovieCinemasMovies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieCinemasMovies",
                table: "MovieCinemasMovies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieCinemas",
                table: "MovieCinemas");

            migrationBuilder.RenameTable(
                name: "MovieCinemasMovies",
                newName: "MovieTheatersMovies");

            migrationBuilder.RenameTable(
                name: "MovieCinemas",
                newName: "MovieTheaters");

            migrationBuilder.RenameIndex(
                name: "IX_MovieCinemasMovies_MovieId",
                table: "MovieTheatersMovies",
                newName: "IX_MovieTheatersMovies_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieTheatersMovies",
                table: "MovieTheatersMovies",
                columns: new[] { "MovieTheaterId", "MovieId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieTheaters",
                table: "MovieTheaters",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieTheatersMovies_Movies_MovieId",
                table: "MovieTheatersMovies",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieTheatersMovies_MovieTheaters_MovieTheaterId",
                table: "MovieTheatersMovies",
                column: "MovieTheaterId",
                principalTable: "MovieTheaters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
