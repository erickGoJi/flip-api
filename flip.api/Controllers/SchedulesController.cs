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
    public class SchedulesController : ControllerBase
    {
        private readonly Db_FlipContext _context;

        public SchedulesController(Db_FlipContext context)
        {
            _context = context;
        }       

        // GET: api/Schedules
        [HttpGet]
        public IActionResult GetSchedules([FromQuery] int? id = null, [FromQuery]int? buildingId = null, 
            [FromQuery]int? amenityId = null, [FromQuery]DateTime? startDate = null, [FromQuery]DateTime? endDate = null, 
            [FromQuery]bool? isPrivate = null, [FromQuery]bool? groupByActivity = null, [FromQuery]int? activityId = null,
            [FromQuery]bool? hasCupo = null, [FromQuery]int? isBookByUserId = null)
        {
            var res = (from sc in _context.Schedules.Include(a => a.Activity)
                       join ac in _context.Activities on sc.ActivityId equals ac.Id
                       join am in _context.Amenities on ac.AmenityId equals am.Id
                       //where sc.Activity.Amenity.BuildingId == (buildingId.HasValue ? buildingId : sc.Activity.Amenity.BuildingId) &&
                       //sc.Id == (id.HasValue ? id : sc.Id)
                       select new
                       {
                           sc.Id, sc.Date, sc.TimeStart, sc.TimeEnd, sc.Activity
                           //,user = new { ac.User.Id, ac.User.Name, ac.User.LastName, ac.User.Avatar }        // CHECK FOR ISSUES
                       }
                     );

            if (id.HasValue) { res = res.Where(r => r.Id == id); }
            if (buildingId.HasValue) { res = res.Where(r => r.Activity.Amenity.BuildingId == buildingId); }   
            if (amenityId.HasValue) { res = res.Where(r => r.Activity.AmenityId == amenityId); }
            if (startDate.HasValue) { res = res.Where(r => r.TimeStart > startDate); }
            if (endDate.HasValue) { res = res.Where(r => r.TimeEnd < endDate); }
            if (isPrivate.HasValue) { res = res.Where(r => r.Activity.Private == isPrivate); }
            if (activityId.HasValue) { res = res.Where(r => r.Activity.Id == activityId); }


            // VALIDACIÓN CUPO ( Para + O Reservado: userId, conseguir )
            if (hasCupo.HasValue) {
                if (hasCupo.Value) {
                    if (isBookByUserId.HasValue) {
                        res = res.Where((r) => r.Activity.QuoteMax > _context.Books.Count(b => b.ScheduleId == r.Id) || 
                            _context.Books.Count(bk => bk.ScheduleId == r.Id && bk.UserId == isBookByUserId) > 0);
                    } else {
                        res = res.Where( (r) => r.Activity.QuoteMax > _context.Books.Count(b => b.ScheduleId == r.Id) );                    
                    }
                }
            }                                                 

            // ONLY RETURN ONE REGISTER FOR ACTIVY EVEN IF THIS HAS 1 OR MORE SCHEDULES (ACTIVE USE WITH START DATE AND/OR END DATE)
            if (groupByActivity.HasValue) {
                if (groupByActivity.Value) {                    
                    var res2 = res.OrderBy(r => r.Date).GroupBy(r => r.Activity).Select(x => x.FirstOrDefault());
                    res2 = res2.OrderBy(r => r.Date);
                    return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = res2 });
                }                
            }

            res = res.OrderBy(r => r.Date);
            if (res == null) {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            } else {                
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = res });
            }
        }

        // GET: api/Schedules/UserBookSchedules
        [Route("UserBookSchedules")]
        [HttpGet("{UserBookSchedules}")]
        public IActionResult UserBookSchedules([FromQuery]bool? hasCupo = null)
        {
            var res = (from sc in _context.Schedules.Include(a => a.Activity)
                       join ac in _context.Activities on sc.ActivityId equals ac.Id
                       join am in _context.Amenities on ac.AmenityId equals am.Id                       
                       select new { sc.Id, sc.Date, sc.TimeStart, sc.TimeEnd, sc.Activity }
                     );            

            if (res == null) {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            } else {
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = res });
            }
        }

        // PUT: api/Schedules/5
        //[HttpPut("{id}")]
        //public IActionResult PutSchedule(int id, Schedule schedule)
        //{
        //    if (!ScheduleExists(id)) { return NotFound(); }            

        //    // VALIDACIONES NO OVERLAP            
        //    var alreadyExist = _context.Schedules.Where(s => s.Date == schedule.Date && (schedule.TimeStart < s.TimeEnd && s.TimeStart < schedule.TimeEnd)).ToList();
        //    if (alreadyExist.Count > 1) {
        //        return Ok(new { result = "Error", detalle = "Already Exists", item = alreadyExist });
        //    } else if (alreadyExist.Count == 1 && alreadyExist[0].Id != id) {
        //        return Ok(new { result = "Error", detalle = "Already Exists", item = alreadyExist });
        //    }

        //    var scheduleDetail = _context.Schedules.FirstOrDefault(a => a.Id == id);
        //    scheduleDetail.Date = schedule.Date;
        //    scheduleDetail.TimeStart = schedule.TimeStart;
        //    scheduleDetail.TimeEnd = schedule.TimeEnd;
        //    _context.Schedules.Update(scheduleDetail);

        //    _context.Entry(schedule).State = EntityState.Modified;

        //    try
        //    {
        //        _context.SaveChanges();
        //        return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = schedule });
        //    }
        //    catch (Exception e) { return NotFound(); }           
        //}

        // PUT: api/Schedules/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSchedule(int id, Schedule schedule)
        {
            //return Ok(new { result = "Success", detalle = "Correct Result", item = schedule });
            _context.Entry(schedule).State = EntityState.Modified;

            if (id != schedule.Id) { return BadRequest(); }
            if (!ScheduleExists(id)) { return NotFound(); }

            // VALIDACIONES NO OVERLAP            
            var alreadyExist = _context.Schedules.Where(
                s => s.Date == schedule.Date && 
                (schedule.TimeStart < s.TimeEnd && s.TimeStart < schedule.TimeEnd) &&
                (s.Activity.AmenityId == schedule.Activity.AmenityId) //&&
                //(s.Activity.AmenityId == _context.Activities.FirstOrDefault(ac => ac.Id == schedule.ActivityId).AmenityId)
            ).ToList();
            if (alreadyExist.Count > 1) {
                //return Ok(new { result = "Error", detalle = "Already Exists", item = alreadyExist });
                return Ok(new { result = "Error", detalle = "Already Exists", item = alreadyExist[0].Date });
            }
            else if (alreadyExist.Count == 1 && alreadyExist[0].Id != id) {
                //return Ok(new { result = "Error", detalle = "Already Exists", item = alreadyExist });
                return Ok(new { result = "Error", detalle = "Already Exists", item = alreadyExist[0].Date });
            }


            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!ScheduleExists(id)) { return NotFound(); }
                else { throw; }
            }
            return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = schedule });
        }

        // POST: api/Schedules
        [HttpPost]
        public IActionResult PostSchedule(Schedule schedule)
        {
            // VALIDACIONES NO OVERLAP           
            var alreadyExist = _context.Schedules.Count(
                s => s.Date == schedule.Date && 
                (schedule.TimeStart < s.TimeEnd && s.TimeStart < schedule.TimeEnd) &&
                (s.Activity.AmenityId == _context.Activities.FirstOrDefault(ac => ac.Id == schedule.ActivityId).AmenityId )
            ) > 0;
            //var alreadyExist = _context.Schedules.Count(s => s.Date == schedule.Date && (schedule.TimeStart < s.TimeEnd && s.TimeStart < schedule.TimeEnd) ) > 0;            
            if (alreadyExist) { return Ok(new { result = "Error", detalle = "Already Exists", item = alreadyExist }); }                  

            _context.Schedules.Add(schedule);

            try {
                _context.SaveChanges();                
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = schedule });                
            } catch (Exception e) { return NotFound();}

            //_context.Schedules.Add(schedule);
            //await _context.SaveChangesAsync();
            //return CreatedAtAction("GetSchedule", new { id = schedule.Id }, schedule);            
        }

        // DELETE: api/Schedules/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Schedule>> DeleteSchedule(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();

            return schedule;
        }

        private bool ScheduleExists(int id)
        {
            return _context.Schedules.Any(e => e.Id == id);
        }
    }
}
