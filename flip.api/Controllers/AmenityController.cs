using System;
using System.Collections.Generic;
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
    public class AmenityController : ControllerBase
    {
        private readonly Db_FlipContext _context;
        public AmenityController(Db_FlipContext context)
        {
            _context = context;
        }

        public class RequestAmenity
        {
            public int idUser { get; set; }
            public int id { get; set; }
        }

        [Route("GetListAmenity")]
        [HttpPost]
        public IActionResult postList([FromBody] RequestAmenity request)
        {
            var res = (from c in _context.Amenities
                       join a in _context.Buildings on c.BuildingId equals a.Id
                       join u in _context.Users on a.Id equals u.BuildingId
                       where u.Id == request.idUser && c.BuildingId == request.id
                       select c
                      );
            //_context.PerkGuides.Where(a => a.Id == Id).ToList();
            if (res == null)
            {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            }
            else
            {
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = res });
            }
        }

        [Route("GetAmenityById")]
        [HttpPost]
        public IActionResult postAmenityById([FromBody] RequestAmenity request)
        {
            var res = (from c in _context.Amenities
                       join a in _context.Buildings on c.BuildingId equals a.Id
                       join u in _context.Users on a.Id equals u.BuildingId
                       where u.Id == request.idUser && c.Id == request.id
                       select c
                      );
            //_context.PerkGuides.Where(a => a.Id == Id).ToList();
            if (res == null)
            {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            }
            else
            {
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = res });
            }
        }

        [Route("GetAmenityByIdBuild")]
        [HttpPost]
        public IActionResult postAmenityByIdBuild([FromBody] RequestAmenity request)
        {
            var res = (from c in _context.Amenities
                       join a in _context.Buildings on c.BuildingId equals a.Id
                       where c.BuildingId == request.id
                       select c
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


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public class AmenityRequestModel {
            public string name { get; set; }
            public string description { get; set; }
            public string photo { get; set; }
        }      

        // GET: api/Amenity/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Amenity>> GetAmenity(int id)
        {
            var amenity = await _context.Amenities.FindAsync(id);

            if (amenity == null)
            {
                return NotFound();
            }

            return amenity;
        }

        // GET: api/Amenity
        [HttpGet]
        public IActionResult GetAmenities([FromQuery] int? id=null, [FromQuery]int? idBuilding=null)        
        {                        
            IQueryable<Amenity> query = _context.Amenities;            
             
            if (id.HasValue) { query = query.Where(a => a.Id == id); }
            if (idBuilding.HasValue) { query = query.Where(a => a.BuildingId == idBuilding); }

            
            if (query == null) {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            } else {
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = query });
            }
        }

        // POST: api/Amenity
        [HttpPost]
        public IActionResult PostAmenity(Amenity amenity)
        {                        
            _context.Amenities.Add(amenity);

            try
            {
                _context.SaveChanges();
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = amenity });
            }
            catch (Exception e) {
                return NotFound();
            }
        }

        // PUT: api/Amenity/5
        [HttpPut("{id}")]
        public IActionResult PutAmenity(int id, AmenityRequestModel amenityRequest)
        {
            var amenity = _context.Amenities.FirstOrDefault(a => a.Id == id);
            if (amenity == null) { return NotFound(); }

            amenity.Name = amenityRequest.name;
            amenity.Description = amenityRequest.description;
            amenity.Photo = amenityRequest.photo;                                    

            _context.Amenities.Update(amenity);            

            try {
                _context.SaveChanges();               
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = amenity });                
            } catch(Exception e) {
                return NotFound();
            }                                                  
        }

        // DELETE: api/Amenity/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Amenity>> DeleteAmenity(int id)
        {
            var amenity = await _context.Amenities.FindAsync(id);
            if (amenity == null) { return NotFound(); }

            _context.Amenities.Remove(amenity);
            await _context.SaveChangesAsync();
            return amenity;
        }

        private bool AmenityExists(int id) { return _context.Amenities.Any(e => e.Id == id); }

    }
}