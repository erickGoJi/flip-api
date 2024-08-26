using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using flip.dal.DB_context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace flip.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionCoversController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly Db_FlipContext _context;        

        public SectionCoversController(IConfiguration config, Db_FlipContext context)
        {
            _config = config;
            _context = context;            
        }

        [Route("Activities")]
        [HttpGet("Activities")]
        public IActionResult Activities() {            
            return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = GetCover(1) });            
        }

        [Route("Amenities")]
        [HttpGet("Amenities")]
        public IActionResult Amenities() {            
            return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = GetCover(2) });
        }

        private String GetCover(int id) {                     
            var res = _context.SectionCovers.FirstOrDefault(r => r.Id == id);
            if (res == null) { return ""; }
            return res.ImageUrl;
        }
    }
}