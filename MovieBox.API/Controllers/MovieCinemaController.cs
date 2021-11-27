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
    public class MovieCinemaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MovieCinemaController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<MovieCinemaDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = _context.MovieCinemas.AsQueryable();
            await HttpContext.InsertParametersPaginationHeader(queryable);
            var entities = await queryable.OrderBy(x => x.Name).Paginate(paginationDTO).ToListAsync();
            return _mapper.Map<List<MovieCinemaDTO>>(entities);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovieCinemaDTO>> Get(int id)
        {
            var movieCinema = await _context.MovieCinemas.FirstOrDefaultAsync(x => x.Id == id);

            if (movieCinema == null)
            {
                return NotFound();
            }

            return _mapper.Map<MovieCinemaDTO>(movieCinema);
        }

        [HttpPost]
        public async Task<ActionResult> Post(MovieCinemaCreationDTO movieCreationDTO)
        {
            var movieCinema = _mapper.Map<MovieCinema>(movieCreationDTO);
            _context.Add(movieCinema);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, MovieCinemaCreationDTO movieCreationDTO)
        {
            var movieCinema = await _context.MovieCinemas.FirstOrDefaultAsync(x => x.Id == id);

            if (movieCinema == null)
            {
                return NotFound();
            }

            movieCinema = _mapper.Map(movieCreationDTO, movieCinema);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var movieCinema = await _context.MovieCinemas.FirstOrDefaultAsync(x => x.Id == id);

            if (movieCinema == null)
            {
                return NotFound();
            }

            _context.Remove(movieCinema);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
