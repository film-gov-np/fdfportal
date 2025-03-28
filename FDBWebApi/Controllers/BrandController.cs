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
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet("{id}")]
        
        public async Task<IActionResult> GetAllMovie([FromRoute] int id)
        {
            var Brands = await _brandService.GetAllBrandAsync(id);
            return Ok(Brands);
        }
    }
}