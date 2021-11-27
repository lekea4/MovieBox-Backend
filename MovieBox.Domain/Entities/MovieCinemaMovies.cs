using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBox.Domain.Entities
{
    public class MovieCinemaMovies
    {
        public int MovieCinemaId { get; set; }
        public int MovieId { get; set; }
        public MovieCinema MovieCinema { get; set; }
        public Movie Movie { get; set; }
    }
}
