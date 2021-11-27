using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBox.Domain.DTOs
{
    public class MoviePutGetDTO
    {
        public MovieDTO Movie { get; set; }
        public List<GenreDTO> SelectedGenres { get; set; }
        public List<GenreDTO> NonSelectedGenres { get; set; }
        public List<MovieCinemaDTO> SelectedMovieCinemas { get; set; }
        public List<MovieCinemaDTO> NonSelectedMovieCinemas { get; set; }
        public List<ActorMovieDTO> Actors { get; set; }
    }
}
