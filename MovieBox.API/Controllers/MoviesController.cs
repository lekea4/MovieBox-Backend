using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieBox.Domain.DTOs;
using MovieBox.Domain.Entities;
using MovieBox.Domain.Helpers;
using MovieBox.Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieBox.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private string container = "movies";

        public MoviesController(AppDbContext context, IMapper mapper,
            IFileStorageService fileStorageService)
        {
            _context = context;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

        [HttpGet]
        public async Task<ActionResult<LandingPageDTO>> Get()
        {
            var top = 6;
            var today = DateTime.Today;

            var upcomingReleases = await _context.Movies
                .Where(x => x.ReleaseDate > today)
                .OrderBy(x => x.ReleaseDate)
                .Take(top)
                .ToListAsync();

            var inCinemas = await _context.Movies
                .Where(x => x.InCinemas)
                .OrderBy(x => x.ReleaseDate)
                .Take(top)
                .ToListAsync();

            var landingPageDTO = new LandingPageDTO();
            landingPageDTO.UpcomingReleases = _mapper.Map<List<MovieDTO>>(upcomingReleases);
            landingPageDTO.InCinemas = _mapper.Map<List<MovieDTO>>(inCinemas);
            return landingPageDTO;
        }

        [HttpGet("PostGet")]
        public async Task<ActionResult<MoviePostGetDTO>> PostGet()
        {
            var movieCinemas = await _context.MovieCinemas.OrderBy(x => x.Name).ToListAsync();
            var genres = await _context.Genres.OrderBy(x => x.Name).ToListAsync();

            var movieCinemasDTO = _mapper.Map<List<MovieCinemaDTO>>(movieCinemas);
            var genresDTO = _mapper.Map<List<GenreDTO>>(genres);

            return new MoviePostGetDTO() { Genres = genresDTO, MovieCinemas= movieCinemasDTO };

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovieDTO>> Get(int id)
        {
            var movie = await _context.Movies
                .Include(x => x.MoviesGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MovieCinemasMovies).ThenInclude(x => x.MovieCinema)
                .Include(x => x.MoviesActors).ThenInclude(x => x.Actor)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<MovieDTO>(movie);
            dto.Actors = dto.Actors.OrderBy(x => x.Order).ToList();
            return dto;
        }

        [HttpGet("filter")]
        public async Task<ActionResult<List<MovieDTO>>> Filter([FromQuery] FilterMoviesDTO filterMoviesDTO)
        {
            var moviesQueryable = _context.Movies.AsQueryable();

            if (!string.IsNullOrEmpty(filterMoviesDTO.Title))
            {
                moviesQueryable = moviesQueryable.Where(x => x.Title.Contains(filterMoviesDTO.Title));
            }

            if (filterMoviesDTO.InCinemas)
            {
                moviesQueryable = moviesQueryable.Where(x => x.InCinemas);
            }

            if (filterMoviesDTO.UpcomingReleases)
            {
                var today = DateTime.Today;
                moviesQueryable = moviesQueryable.Where(x => x.ReleaseDate > today);
            }

            if (filterMoviesDTO.GenreId != 0)
            {
                moviesQueryable = moviesQueryable
                    .Where(x => x.MoviesGenres.Select(y => y.GenreId)
                    .Contains(filterMoviesDTO.GenreId));
            }

            await HttpContext.InsertParametersPaginationHeader(moviesQueryable);
            var movies = await moviesQueryable.OrderBy(x => x.Title).Paginate(filterMoviesDTO.PaginationDTO)
                .ToListAsync();
            return _mapper.Map<List<MovieDTO>>(movies);
        }


        [HttpPost]
        public async Task<ActionResult<int>> Post([FromForm] MovieCreationDTO movieCreationDTO)
        {
            var movie = _mapper.Map<Movie>(movieCreationDTO);

            if (movieCreationDTO.Poster != null)
            {
                movie.Poster = await _fileStorageService.SaveFile(container, movieCreationDTO.Poster);
            }

            AnnotateActorsOrder(movie);
            _context.Add(movie);
            await _context.SaveChangesAsync();
            return movie.Id;
        }

        [HttpGet("putget/{id:int}")]
        public async Task<ActionResult<MoviePutGetDTO>> PutGet(int id)
        {
            var movieActionResult = await Get(id);
            if (movieActionResult.Result is NotFoundResult) { return NotFound(); }

            var movie = movieActionResult.Value;

            var genresSelectedIds = movie.Genres.Select(x => x.Id).ToList();
            var nonSelectedGenres = await _context.Genres.Where(x => !genresSelectedIds.Contains(x.Id))
                .ToListAsync();

            var movieCinemasIds = movie.MovieCinemas.Select(x => x.Id).ToList();
            var nonSelectedMovieCinemas = await _context.MovieCinemas.Where(x =>
            !movieCinemasIds.Contains(x.Id)).ToListAsync();

            var nonSelectedGenresDTOs = _mapper.Map<List<GenreDTO>>(nonSelectedGenres);
            var nonSelectedMovieCinemasDTO = _mapper.Map<List<MovieCinemaDTO>>(nonSelectedMovieCinemas);

            var response = new MoviePutGetDTO();
            response.Movie = movie;
            response.SelectedGenres = movie.Genres;
            response.NonSelectedGenres = nonSelectedGenresDTOs;
            response.SelectedMovieCinemas = movie.MovieCinemas;
            response.NonSelectedMovieCinemas = nonSelectedMovieCinemasDTO;
            response.Actors = movie.Actors;
            return response;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] MovieCreationDTO movieCreationDTO)
        {
            var movie = await _context.Movies.Include(x => x.MoviesActors)
                .Include(x => x.MoviesGenres)
                .Include(x => x.MovieCinemasMovies)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            movie = _mapper.Map(movieCreationDTO, movie);

            if (movieCreationDTO.Poster != null)
            {
                movie.Poster = await _fileStorageService.EditFile(container, movieCreationDTO.Poster,
                    movie.Poster);
            }

            AnnotateActorsOrder(movie);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private void AnnotateActorsOrder(Movie movie)
        {
            if (movie.MoviesActors != null)
            {
                for (int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i;
                }
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(x => x.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            _context.Remove(movie);
            await _context.SaveChangesAsync();
            await _fileStorageService.DeleteFile(movie.Poster, container);
            return NoContent();
        }
    }
}
