using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBox.Domain.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 75)]
        [Required]
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Trailer { get; set; }
        public bool InCinemas { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Poster { get; set; }

        //Relationships 
        public List<MoviesGenres> MoviesGenres { get; set; }
        public List<MovieCinemaMovies> MovieCinemasMovies { get; set; }
        public List<MovieActor> MoviesActors { get; set; }
    }
}
