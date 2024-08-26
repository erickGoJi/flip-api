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
    public class HistoricalServicesController : ControllerBase
    {
        private readonly Db_FlipContext _context;

        public HistoricalServicesController(Db_FlipContext context)
        {
            _context = context;
        }

        // GET: api/HistoricalServices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistoricalService>>> GetHistoricalServices()
        {
            return await _context.HistoricalServices.ToListAsync();
        }

        // Used for services view in the app
        // GET: api/Services/GetBookedAndBuilding    
        [Route("GetBookedAndBuilding")]
        [HttpGet("GetBookedAndBuilding")]
        public IActionResult GetBookedAndBuilding([FromQuery] int userId, [FromQuery] int buildingId)
        {
            DateTime date1 = DateTime.Now;

            var consult = (from a in _context.Bookings
                           where a.IdUser == userId
                           select new { idBooking = (a.Id), idUser = (a.IdUser) }).FirstOrDefault();
            IQueryable<ServiceBooking> res = _context.ServiceBookings
                .Where(r => r.IdBooking == consult.idBooking && r.DateEnd.Date >= date1.Date);

            IQueryable<Service> services = _context.Services.Where(r => r.BuildingId == buildingId);
            
            var response = services.Select(r => new {
                r.Id, r.Name, r.Icon, booked = res.Any(x => x.IdService == r.Id)
            });
            if (res == null) {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            }
            else {
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = response });
            }
        }


        // PUT: api/HistoricalServices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHistoricalService(int id, HistoricalService historicalService)
        {
            if (id != historicalService.Id)
            {
                return BadRequest();
            }

            _context.Entry(historicalService).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HistoricalServiceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/HistoricalServices
        [HttpPost]
        public async Task<ActionResult<HistoricalService>> PostHistoricalService(HistoricalService historicalService)
        {
            _context.HistoricalServices.Add(historicalService);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHistoricalService", new { id = historicalService.Id }, historicalService);
        }

        // DELETE: api/HistoricalServices/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<HistoricalService>> DeleteHistoricalService(int id)
        {
            var historicalService = await _context.HistoricalServices.FindAsync(id);
            if (historicalService == null)
            {
                return NotFound();
            }

            _context.HistoricalServices.Remove(historicalService);
            await _context.SaveChangesAsync();

            return historicalService;
        }

        private bool HistoricalServiceExists(int id)
        {
            return _context.HistoricalServices.Any(e => e.Id == id);
        }
    }
}
