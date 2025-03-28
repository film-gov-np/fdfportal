using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FDBWebApi.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FDBWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet("{id}")]
        
        public async Task<IActionResult> GetAllMovie([FromRoute] int id)
        {
            var Movies = await _movieService.GetAllMovieAsync(id);
            return Ok(Movies);
        }
    }
}