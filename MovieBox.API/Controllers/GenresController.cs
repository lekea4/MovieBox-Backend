using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieBox.Domain.Entities;
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

        public GenresController(ILogger<GenresController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet] // api/genres
        public async Task<ActionResult<List<Genre>>> Get()
        {
            _logger.LogInformation("Getting all the genres");

            return await _context.Genres.ToListAsync();
        }

        [HttpGet("{Id:int}", Name = "getGenre")] // api/genres/example
        public ActionResult<Genre> Get(int Id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Genre genre)
        {
            await _context.AddAsync(genre);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut]
        public ActionResult Put([FromBody] Genre genre)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public ActionResult Delete()
        {
            throw new NotImplementedException();
        }
    }
}
