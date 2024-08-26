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
    public class ServicesController : ControllerBase
    {
        private readonly Db_FlipContext _context;

        public ServicesController(Db_FlipContext context)
        {
            _context = context;
        }

        // GET: api/Services
        [HttpGet]
        public IActionResult GetServices([FromQuery] int? id = null, [FromQuery] int? buildingId = null)
        {
            IQueryable<Service> res = _context.Services;

            if (id.HasValue) { res = res.Where(r => r.Id == id); }
            if (buildingId != null) { res = res.Where(r => r.BuildingId == buildingId); }            

            var response = res.Select(r => new {
                r.Id, r.Name, r.Description, r.Icon, r.Photo, r.Provider, r.Price, r.PriceUnit, r.BuildingId
            });

            if (res == null) {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            }
            else {
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = response });
            }
        }

        // PUT: api/Services
        [HttpPut]
        public IActionResult PutService(Service service)
        {
            Service serviceDetail = _context.Services.Find(service.Id);
            if (serviceDetail == null) { return NotFound(); }

            serviceDetail.Name = service.Name;
            serviceDetail.Description = service.Description;
            serviceDetail.Icon = service.Icon;
            serviceDetail.Photo = service.Photo;
            serviceDetail.Provider = service.Provider;
            serviceDetail.Price = service.Price;
            serviceDetail.PriceUnit = service.PriceUnit;

            _context.Services.Update(serviceDetail);

            try
            {
                _context.SaveChanges();
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = serviceDetail.Id });
            }
            catch (Exception e) { return Ok(new { result = "Error", detalle = "Error", item = 0 }); }

        }

        // POST: api/Services
        [HttpPost]
        public IActionResult PostService(Service service)
        {
            _context.Services.Add(service);

            try
            {
                _context.SaveChanges();
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = service.Id });
            }
            catch (Exception e) { return Ok(new { result = "Error", detalle = "Error", item = 0 }); }
        }

        // DELETE: api/Services/5
        [HttpDelete("{id}")]
        public IActionResult DeleteService(int id)
        {
            var service = _context.Services.Find(id);
            if (service == null)
            {
                return NotFound();
            }

            _context.Services.Remove(service);    
            
            try
            {
                _context.SaveChanges();
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = id });
            }
            catch (Exception e) { return Ok(new { result = "Error", detalle = "Error", item = 0 }); }
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    }
}
