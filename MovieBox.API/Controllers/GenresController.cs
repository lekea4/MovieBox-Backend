using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    public class GenresController : ControllerBase
    {
        private readonly ILogger<GenresController> _logger;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GenresController(ILogger<GenresController> logger, AppDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet] // api/genres
        public async Task<ActionResult<List<GenreDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = _context.Genres.AsQueryable();
            await HttpContext.InsertParametersPaginationHeader(queryable);
            var genres = await queryable.OrderBy(x => x.Name).Paginate(paginationDTO).ToListAsync();
            return _mapper.Map<List<GenreDTO>>(genres);
        }

        [HttpGet("all")] //api/genre
        public async Task<ActionResult<List<GenreDTO>>> Get()
        {
            var genre = await _context.Genres.OrderBy(x => x.Name).ToListAsync();
            return _mapper.Map<List<GenreDTO>>(genre);
        }

        [HttpGet("{Id:int}")] // api/genres/example
        public async Task<ActionResult<GenreDTO>> Get(int Id)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(x => x.Id == Id);

            if (genre == null)
            {
                return NotFound();
            }
            return _mapper.Map<GenreDTO>(genre);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GenreCreationDTO genreCreationDTO)
        {
            var genre = _mapper.Map<Genre>(genreCreationDTO);
            await _context.AddAsync(genre);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] GenreCreationDTO genreCreationDTO)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(x => x.Id == id);

            if (genre == null)
            {
                return NotFound();
            }

            genre = _mapper.Map(genreCreationDTO, genre);
            await _context.SaveChangesAsync();
            return NoContent();

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await _context.Genres.AnyAsync(x => x.Id == id);

            if (!exists)
            {
                return NotFound();
            }

            _context.Remove(new Genre() { Id = id });
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

