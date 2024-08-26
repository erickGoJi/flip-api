using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using flip.biz.Entities;
using flip.dal.DB_context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace flip.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerkGuideController : ControllerBase
    {
        private readonly Db_FlipContext _context;
        public PerkGuideController(Db_FlipContext context)
        {
            _context = context;
        }

        public class RequestPerkGuide
        {
            [Required(ErrorMessage = "UserId is required")]
            public int userid { get; set; }
            [Required(ErrorMessage = "Id is required")]
            public int id { get; set; }
        }

        [HttpGet]
        public ActionResult<List<PerkGuide>> Get()
        {
            var res = _context.PerkGuides.Where(a => a.PackCategoryId == 1).ToList();
            if (res == null)
            {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            }
            else
            {
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = res });
            }
            //return res;
        }        

        [Route("PerksByCategory")]
        [HttpPost("PerksByCategory")]
        public ActionResult<List<PerkGuide>> Post([FromBody] RequestPerkGuide request)
        {            
              var  res = (from c in _context.PerkGuides
                       join a in _context.Buildings on c.BuildingId equals a.Id
                       join u in _context.Users on a.Id equals u.BuildingId
                       where u.Id == request.userid && c.PackCategoryId == (request.id == 0 ? c.PackCategoryId : request.id)
                       select new
                       {
                           c.Id, c.Name, c.Description, c.StreetAddress, c.City, c.StateProvincy, c.Zip, c.Country, c.Latitude, c.Longitude,
                           c.PackCategory,
                           c.Photo
                       }
                      );
                      
            if (res == null) {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            } else {
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = res });
            }
        }

        [Route("PostPerkGuideById")]
        [HttpPost("PostPerkGuideById")]
        public IActionResult PostPerkGuideById([FromBody] RequestPerkGuide request)
        {
            var res = (from c in _context.PerkGuides.Include(a => a.GalleryPerks)
                       join a in _context.Buildings on c.BuildingId equals a.Id
                       join u in _context.Users on a.Id equals u.BuildingId                       
                       where u.Id == request.userid && c.Id == request.id
                       select new
                       {                                       
                           c.Id, c.Name, c.Description, c.StreetAddress, c.City, c.StateProvincy, c.Zip, c.Country, c.Latitude, c.Longitude,
                           c.PackCategory,
                           c.GalleryPerks                         
                       }
                      );                
            if (res == null)
            {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            }
            else
            {
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = res });
            }
        }

        [Route("GetCategoriesByUser")]
        [HttpPost("GetCategoriesByUser")]
        public IActionResult GetCategoriesByUser([FromBody] RequestPerkGuide request)
        {
            var res = (from c in _context.PerkGuides                       
                       join pc in _context.PerkCategories on c.PackCategoryId equals pc.Id
                       join a in _context.Buildings on c.BuildingId equals a.Id                       
                       join u in _context.Users on a.Id equals u.BuildingId                       
                       where u.Id == request.userid                       
                       group new { pc, c } by c.PackCategoryId into d
                       select new
                       {
                           cId = d.Key, categoryName = d.First().pc.Name, icon = d.First().pc.Icon
                       }            
                      );
            
            if (res == null)
            {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            }
            else
            {
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = res });
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///

        // GET: PerkGuide/GetCategories
        [Route("GetCategories")]
        [HttpGet("GetCategories")]
        public IActionResult GetCategories([FromQuery] int? id = null, [FromQuery] string name = null)
        {
            var res = _context.PerkCategories.Select(r => new { r.Id, r.Name, r.Icon });

            if (id.HasValue) { res = res.Where(r => r.Id == id); }
            if (name != null) { res = res.Where(r => r.Name == name); }

            if (res == null)
            {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            }
            else
            {
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = res });
            }
        }

        // GET: PerkGuide/GetPerks
        [Route("GetPerks")]
        [HttpGet("GetPerks")]
        public IActionResult GetPerks([FromQuery] int? id = null, [FromQuery] int? buildingId = null, [FromQuery] int? packCategoryId = null)
        {
            IQueryable<PerkGuide> res = _context.PerkGuides;

            if (id.HasValue) { res = res.Where(r => r.Id == id); }
            if (buildingId != null) { res = res.Where(r => r.BuildingId == buildingId); }
            if (packCategoryId != null) { res = res.Where(r => r.PackCategoryId == packCategoryId); }    
            
            var response = res.Select(r => new {
                r.Id, r.Name, r.Description, r.StreetAddress, r.City, r.StateProvincy, r.Zip, r.Country, r.Latitude, r.Longitude, r.Photo, r.BuildingId, r.PackCategoryId,             
            });

            if (res == null) {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            }
            else {
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = response });
            }
        }

        // GET: PerkGuide/GetPerksWithGallery
        [Route("GetPerksWithGallery")]
        [HttpGet("GetPerksWithGallery")]
        public IActionResult GetPerksWithGallery([FromQuery] int? id = null, [FromQuery] int? buildingId = null, [FromQuery] int? packCategoryId = null)
        {
            IQueryable<PerkGuide> res = _context.PerkGuides.Include(r => r.GalleryPerks);

            if (id.HasValue) { res = res.Where(r => r.Id == id); }
            if (buildingId != null) { res = res.Where(r => r.BuildingId == buildingId); }
            if (packCategoryId != null) { res = res.Where(r => r.PackCategoryId == packCategoryId); }

            var response = res.Select(r => new {
                r.Id, r.Name, r.Description, r.StreetAddress, r.City, r.StateProvincy, r.Zip, r.Country, r.Latitude, r.Longitude, r.Photo, r.BuildingId, r.PackCategoryId,
                r.GalleryPerks
            });

            if (res == null) {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            } else {
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = response });
            }
        }


        // POST: PerkGuide/AddPerk
        [Route("AddPerk")]
        [HttpPost("AddPerk")]
        public IActionResult AddPerk([FromBody] PerkGuide perkGuide)
        {
            _context.PerkGuides.Add(perkGuide);

            try {
                _context.SaveChanges();
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = perkGuide.Id});
            }
            catch (Exception e) { return Ok(new { result = "Error", detalle = "Error", item = 0 }); }
        }

        // PUT: PerkGuide/UpdatePerk
        [Route("UpdatePerk")]
        [HttpPut("UpdatePerk")]
        public IActionResult UpdatePerk([FromBody] PerkGuide perkGuide)
        {
            var perkDetail = _context.PerkGuides.Find(perkGuide.Id);
            if (perkDetail == null) {
                return NotFound();
            }

            perkDetail.Name = perkGuide.Name;
            perkDetail.Description = perkGuide.Description;
            perkDetail.StreetAddress = perkGuide.StreetAddress;
            perkDetail.City = perkGuide.City;
            perkDetail.StateProvincy = perkGuide.StateProvincy;
            perkDetail.Zip = perkGuide.Zip;
            perkDetail.Country = perkGuide.Country;
            perkDetail.Latitude = perkGuide.Latitude;
            perkDetail.Longitude = perkGuide.Longitude;
            perkDetail.PackCategoryId = perkGuide.PackCategoryId;
            perkDetail.Photo = perkGuide.Photo;

            perkDetail.GalleryPerks = perkGuide.GalleryPerks;

            var gallery = _context.GalleryPerks.Where(r => r.PerkGuideId == perkGuide.Id);
            _context.GalleryPerks.RemoveRange(gallery);
            _context.PerkGuides.Update(perkDetail);

            try
            {
                _context.SaveChanges();
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = perkGuide.Id });
            }
            catch (Exception e) { return Ok(new { result = "Error", detalle = "Error", item = 0 }); }
        }

        // DELETE: PerkGuide/AddPerk        
        [HttpDelete("{id}")]
        public IActionResult DeletePerk(int  id)
        {
            var perk = _context.PerkGuides.Find(id);
            _context.PerkGuides.Remove(perk);

            try
            {
                _context.SaveChanges();
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = id });
            }
            catch (Exception e) { return Ok(new { result = "Error", detalle = "Error", item = 0 }); }
        }
    }
}