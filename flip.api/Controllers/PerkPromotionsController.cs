using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using flip.biz.Entities;
using flip.dal.DB_context;

namespace flip.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerkPromotionsController : ControllerBase
    {
        private readonly Db_FlipContext _context;

        public PerkPromotionsController(Db_FlipContext context)
        {
            _context = context;
        }

        // GET: PerkPromotions/GetPromotions
        [Route("GetPromotions")]
        [HttpGet("GetPromotions")]
        public IActionResult GetPromotions([FromQuery] int? id = null, [FromQuery]DateTime? startDate = null, [FromQuery]DateTime? endDate = null, [FromQuery] int? perkGuideId = null)
        {
            IQueryable<PerkPromotion> res = _context.PerkPromotions;

            if (id.HasValue) { res = res.Where(r => r.Id == id); }
            //if (buildingId != null) { res = res.Where(r => r.BuildingId == buildingId); }
            if (startDate.HasValue) { res = res.Where(r => r.StartDate > startDate); }
            if (endDate.HasValue) { res = res.Where(r => r.EndDate < endDate); }
            if (perkGuideId != null) { res = res.Where(r => r.PerkGuideId == perkGuideId); }

            var response = res.Select(r => new {
                r.Id, r.Name, r.Description, r.StartDate, r.EndDate, r.Photo, r.PerkGuideId
            });

            if (res == null) {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            }
            else {
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = response });
            }
        }

        // GET: PerkPromotions/GetBuildingPromotions
        [Route("GetBuildingPromotions")]
        [HttpGet("GetBuildingPromotions")]
        public IActionResult GetBuildingPromotions([FromQuery] int buildingId)
        {
            IQueryable<PerkPromotion> res = _context.PerkPromotions.Include(r => r.PerkGuide.Name)
                //.Where(r => r.PerkGuide.BuildingId == buildingId && r.StartDate < DateTime.Now);
                .Where(r => r.PerkGuide.BuildingId == buildingId && (DateTime.Now < r.EndDate));            

            var response = res.Select(r => new {
                r.Id, r.Name, r.Description, r.StartDate, r.EndDate, r.Photo, r.PerkGuideId,
                perkName = r.PerkGuide.Name, perkLatitude = r.PerkGuide.Latitude, perkLongitude = r.PerkGuide.Longitude
            });

            if (res == null)
            {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            }
            else
            {
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = response });
            }
        }

        // GET: PerkPromotions/GetPerkPromotions
        [Route("GetPerkPromotions")]
        [HttpGet("GetPerkPromotions")]
        public IActionResult GetPerkPromotions([FromQuery] int perkGuideId)
        {
            IQueryable<PerkPromotion> res = _context.PerkPromotions.Include(r => r.PerkGuide.Name)
                .Where(r => r.PerkGuideId == perkGuideId && DateTime.Now < r.EndDate);

            var response = res.Select(r => new {
                r.Id, r.Name, r.Description, r.StartDate, r.EndDate, r.Photo, r.PerkGuideId, perkName = r.PerkGuide.Name
            });

            if (res == null)
            {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            }
            else
            {
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = response });
            }
        }

        // POST: PerkPromotions/AddPromotion
        [Route("AddPromotion")]
        [HttpPost("AddPromotion")]
        public IActionResult AddPromotion([FromBody] PerkPromotion perkPromotion)
        {
            _context.PerkPromotions.Add(perkPromotion);

            try
            {
                _context.SaveChanges();
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = perkPromotion.Id });
            }
            catch (Exception e) { return Ok(new { result = "Error", detalle = "Error", item = 0 }); }
        }

        // Put: PerkPromotions/EditPromotion
        [Route("EditPromotion")]
        [HttpPut("EditPromotion")]
        public IActionResult EditPromotion([FromBody] PerkPromotion perkPromotion)
        {
            var promotionDetail = _context.PerkPromotions.Find(perkPromotion.Id);
            if (promotionDetail == null) {
                return NotFound();
            }

            promotionDetail.Name = perkPromotion.Name;
            promotionDetail.Description = perkPromotion.Description;
            promotionDetail.StartDate = perkPromotion.StartDate;
            promotionDetail.EndDate = perkPromotion.EndDate;
            promotionDetail.Photo = perkPromotion.Photo;
            _context.PerkPromotions.Update(promotionDetail);

            try
            {
                _context.SaveChanges();
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = perkPromotion.Id });
            }
            catch (Exception e) { return Ok(new { result = "Error", detalle = "Error", item = 0 }); }
        }

        // DELETE: PerkPromotions        
        [HttpDelete("{id}")]
        public IActionResult DeletePromotion(int id)
        {
            var promotion = _context.PerkPromotions.Find(id);
            _context.PerkPromotions.Remove(promotion);

            try
            {
                _context.SaveChanges();
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = id });
            }
            catch (Exception e) { return Ok(new { result = "Error", detalle = "Error", item = 0 }); }
        }
    }
}
