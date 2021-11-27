using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBox.Domain.DTOs
{
    public class LandingPageDTO
    {
        public List<MovieDTO> InCinemas { get; set; }
        public List<MovieDTO> UpcomingReleases { get; set; }
    }
}
