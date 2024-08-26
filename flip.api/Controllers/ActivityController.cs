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
    public class ActivityController : ControllerBase
    {
        private readonly Db_FlipContext _context;

        public ActivityController(Db_FlipContext context)
        {
            _context = context;
        }

        public class RequestAmenity
        {
            [Required(ErrorMessage = "idUser is required")]
            public int idUser { get; set; }
            [Required(ErrorMessage = "Id is required")]
            public int id { get; set; }
        }

        [Route("GetListActivities")]
        [HttpPost]
        public IActionResult postList([FromBody] RequestAmenity request)
        {
            if (ModelState.IsValid)
            {
                var res = (from c in _context.Activities
                           join a in _context.Amenities on c.AmenityId equals a.Id
                           join u in _context.Buildings on a.BuildingId equals u.Id
                           join b in _context.Users on u.Id equals b.BuildingId
                           where b.Id == request.idUser
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
            } else
            {
                return Ok(new { result = "Error", detalle = "Error", item = ModelState });
            }

        }

        [Route("GetActivityById")]
        [HttpPost]
        public IActionResult postAmenityById([FromBody] RequestAmenity request)
        {
            var res = (from c in _context.Activities
                       join a in _context.Amenities on c.AmenityId equals a.Id
                       join u in _context.Buildings on a.BuildingId equals u.Id
                       join b in _context.Users on u.Id equals b.BuildingId
                       where b.Id == request.idUser && c.Id == request.id
                       select new {                           
                           c.Id, c.Name, c.Description, c.Photo, c.QuoteMax, c.AmenityId, c.Amenity                            
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

        [Route("GetScheduleById")]
        [HttpPost]
        public IActionResult postScheduleById([FromBody] RequestAmenity request)
        {
            var res = (from c in _context.Schedules                       
                       where c.Id == request.id
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

        [Route("PostActivity")]
        [HttpPost("{PostActivity}")]
        public IActionResult PostActivity([FromBody] Activity activities)
        {
            if(activities != null)
            {
                _context.Activities.Add(activities);
                _context.SaveChanges();
                return Ok(new { result = "Success", detalle = "Activity dado de alta", idActivity = _context.Activities.LastOrDefault(a => a.Id > 0).Id });
            } else
            {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });

            }
        }

        [Route("PostBookActivity")]
        [HttpPost("{PostBookActivity}")]
        public IActionResult PostBookActivity([FromBody] List<Book> log)
        {
            if (log != null)
            {
                for (int i = 0; i < log.Count; i++)
                {
                    _context.Books.Add(log[i]);
                    _context.SaveChanges();
                }
                return Ok(new { result = "Success", detalle = "Activity dado de alta", item = 0 });
            }
            else
            {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            }
        }

        [Route("PostBookAmenity")]
        [HttpPost("{PostBookAmenity}")]
        public IActionResult PostBookAmenity([FromBody] Book book)
        {
            if (book != null)
            {
                _context.Books.Add(book);
                _context.SaveChanges();
                return Ok(new { result = "Success", detalle = "Activity dado de alta", item = 0 });
            }
            else
            {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            }            
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet]
        public IActionResult GetActivity([FromQuery] int? id = null, [FromQuery]int? buildingId = null, [FromQuery]int? amenityId = null, [FromQuery]bool? isPrivate = null)
        {            
            var res = ( from ac in _context.Activities.Include(a => a.Amenity)            
                        join am in _context.Amenities on ac.AmenityId equals am.Id
                        select new {
                            ac.Id, ac.Name, ac.Description, ac.Photo, ac.QuoteMax, ac.Private, ac.Amenity,
                            user = new { ac.User.Id, ac.User.Name, ac.User.LastName, ac.User.Avatar }
                        }                    
            );

            if (id.HasValue) { res = res.Where(r => r.Id == id); }
            if (buildingId.HasValue) { res = res.Where(r => r.Amenity.BuildingId == buildingId); }
            if (amenityId.HasValue) { res = res.Where(r => r.Amenity.Id == amenityId); }    
            if (isPrivate.HasValue) { res = res.Where(r => r.Private == isPrivate); }                             

            if (res == null) {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            } else {
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = res });
            }            
        }

        // POST: api/Activity
        [HttpPost]
        public IActionResult PostActivitySchedule(Activity activity)
        {
            // VALIDACIONES NEW startTime no sea una FECHA ANTERIOR A ACTUAL
            foreach (var schedule in activity.Schedules)
            {
                if (schedule.TimeStart < DateTime.Now)
                {
                    return Ok(new { result = "Error", detalle = "Start time is before current hour", item = activity });
                }
            }
            
            // VALIDACIONES NO OVERLAP                 
            foreach (var schedule in activity.Schedules)
            {
                var alreadyExist = _context.Schedules.Where(
                    s => s.Date == schedule.Date &&
                    (schedule.TimeStart < s.TimeEnd && s.TimeStart < schedule.TimeEnd) &&
                    (activity.AmenityId == s.Activity.AmenityId)
                ).ToList();
                if (alreadyExist.Count > 0) {
                    //return Ok(new { result = "Error", detalle = "Schedule Not Available", item = alreadyExist });
                    string alreadySchedule = alreadyExist[0].TimeStart.ToString("dd/MM/yyyy HH:mm") + " - " + alreadyExist[0].TimeEnd.ToString("HH:mm");
                    return Ok(new { result = "Error", detalle = "Event already schedule at " + alreadySchedule, item = alreadyExist[0] });                    
                }
            }
            
            _context.Activities.Add(activity);
            try {
                _context.SaveChanges();
                //return Ok(new { result = "Success", detalle = "postActivity schedule", item = activity });
                return Ok(new
                {
                    result = "Success",
                    detalle = "postActivity schedule",
                    item = new { activity.Id, activity.Name }
                });
            } catch(Exception e) {
                return NotFound();
            }            
        }

        //PUT: api/Activity/5
        [HttpPut("{id}")]
        public IActionResult PutSchedule(int id, Activity activity)
        {
            
            if (!ActivityExists(id)) { return NotFound(); }            

            // VALIDACIONES NEW startTime no sea una FECHA ANTERIOR A ACTUAL
            foreach (var schedule in activity.Schedules)
            {
                if (schedule.TimeStart < DateTime.Now) {
                    //return Ok(new { result = "Error", detalle = "Schedule Not Available", item = activity });
                    return Ok(new { result = "Error", detalle = "Start time is before current hour", item = schedule });
                }
            }

            // Schedules that were going to remove based on the activity.schedules received in the service and the differenche with the activityDetail.schedules
            var activityDetail = _context.Activities.Include(r => r.Schedules).FirstOrDefault(a => a.Id == id);
            var schedulesToRemove = activityDetail.Schedules.Where(r => !activity.Schedules.Any(r2 => r2.Id == r.Id)); /*R DeleteSchedule*/
            _context.Schedules.RemoveRange(schedulesToRemove); /*R DeleteSchedule*/

            // VALIDACIONES NO OVERLAP                 
            foreach (var schedule in activity.Schedules)
            {
                var alreadyExist = _context.Schedules.Where(
                    //s => s.Date == schedule.Date &&
                    s => 
                    (schedule.TimeStart < s.TimeEnd && s.TimeStart < schedule.TimeEnd) &&
                    (activity.AmenityId == s.Activity.AmenityId)
                ).ToList();
                if (alreadyExist.Count > 1) {

                    // VALIDACION para saber si esos schedules que ya existeorrar coinciden con los ya existentes    alreadyExist.Count(r => schedulesToRemove.Any(r2 => r2.Id == r.Id))
                    // si la cantidad de estos 2 arreglos es la misma, quiere decir que todos los schedules ya existentes se van eliminar, por lo tanto no hay conflicto de OVERLAP
                    // en caso contrario existe 1+ schedules en los que hay conflicto de OVERLAP
                    if (alreadyExist.Count(r => schedulesToRemove.Any(r2 => r2.Id == r.Id)) == alreadyExist.Count) { continue; } /*R DeleteSchedule*/

                    int index = alreadyExist.FindIndex(r => r.Id != schedule.Id); //se encuentran dentro de la lista de horarios que se van a eliminar,
                    // tenemos los horarios que ya existen (var alreadyExist)
                    // tenemos el arreglo donde los horarios que se van a b
                    string alreadySchedule = alreadyExist[index].TimeStart.ToString("dd/MM/yyyy HH:mm") + " - " + alreadyExist[index].TimeEnd.ToString("HH:mm");
                    return Ok(new { result = "Error", detalle = "Event already schedule at " + alreadySchedule, item = alreadyExist[index] });
                } else if (alreadyExist.Count == 1 && alreadyExist[0].Id != schedule.Id) {
                    
                    if (alreadyExist.Count(r => schedulesToRemove.Any(r2 => r2.Id == r.Id)) == alreadyExist.Count) { continue; } /*R DeleteSchedule*/

                    string alreadySchedule = alreadyExist[0].TimeStart.ToString("dd/MM/yyyy HH:mm") + " - " + alreadyExist[0].TimeEnd.ToString("HH:mm");
                    return Ok(new { result = "Error", detalle = "Event already schedule at " + alreadySchedule, item = alreadyExist[0] });
                }
            }
            
            // Modifying the Activity Information
            activityDetail.Name = activity.Name;
            activityDetail.Description = activity.Description;
            activityDetail.Photo = activity.Photo;
            activityDetail.QuoteMax = activity.QuoteMax;
            _context.Activities.Update(activityDetail);            

            foreach (var schedule in activity.Schedules)
            {
                // New Schedules to add to this activity
                if (schedule.Id == 0) {
                    schedule.ActivityId = activityDetail.Id;
                    _context.Schedules.Add(schedule);
                }
                // Modifying the Schedules that already exists. (Regardless have change or not)
                else
                {
                    var scheduleDetail = _context.Schedules.FirstOrDefault(a => a.Id == schedule.Id);
                    // VALIDACION startTimeOriginal no sea una FECHA ANTERIOR A ACTUAL
                    if (scheduleDetail.TimeStart < DateTime.Now)
                    {
                        return Ok(new { result = "Error", detalle = "Schedule Not Available", item = activity });
                    }

                    scheduleDetail.Date = schedule.Date;
                    scheduleDetail.TimeStart = schedule.TimeStart;
                    scheduleDetail.TimeEnd = schedule.TimeEnd;
                    _context.Schedules.Update(scheduleDetail);
                }                
            }

            //SchedulesController SchedulesController = new SchedulesController(_context);
            //Schedule schedule = new Schedule();
            //schedule.Date = new DateTime();
            //SchedulesController.PostSchedule(schedule);

            try
            {
                _context.SaveChanges();
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = activity });
            }
            catch (Exception e) { return NotFound(e); }
        }        

        // PUT: api/Activity/5        
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutActivity(int id, Activity activity)
        //{
        //    if (id != activity.Id) { return BadRequest(); }

        //    _context.Entry(activity).State = EntityState.Modified;

        //    try { await _context.SaveChangesAsync(); }
        //    catch (DbUpdateConcurrencyException) {
        //        if (!ActivityExists(id)) { return NotFound(); }
        //        else { throw; }
        //    }
        //    return NoContent();
        //}

        // DELETE: api/Activity/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Activity>> DeleteActivity(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null) { return NotFound(); }
           
            _context.Activities.Remove(activity);                        
            await _context.SaveChangesAsync();
            return activity;
        }
        private bool ActivityExists(int id)
        {
            return _context.Activities.Any(e => e.Id == id);
        }
    }
}