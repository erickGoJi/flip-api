using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using flip.biz.Entities;
using flip.dal.DB_context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace flip.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GalleryPerkController : ControllerBase
    {
        private readonly Db_FlipContext _context;

        public GalleryPerkController(Db_FlipContext context)
        {
            _context = context;

        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public ActionResult<List<GalleryPerk>> Get(int id)
        {
            var res = _context.GalleryPerks.Where(a => a.PerkGuideId == id).ToList();
            return res;
        }
    }
}