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
    public class TheaterController : ControllerBase
    {
        private readonly ITheaterService _theaterService;

        public TheaterController(ITheaterService theaterService)
        {
            _theaterService = theaterService;
        }

        [HttpGet("{id}")]
        
        public async Task<IActionResult> GetAllTheater([FromRoute] int id)
        {
            var result = await _theaterService.GetAllTheaterAsync(id);
            return Ok(result);
        }
    }
}