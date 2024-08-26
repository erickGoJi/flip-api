using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using flip.biz.Entities;
using flip.dal.DB_context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace flip.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertsController : ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly Db_FlipContext _context;
        private readonly IHubContext<ChatAlertHub> _chatHubContext;

        public AlertsController(IConfiguration config, Db_FlipContext context, IHubContext<ChatAlertHub> chatHubContext)
        {
            _config = config;
            _context = context;
            _chatHubContext = chatHubContext;
        }

        // GET: api/Alerts/Categories
        [Route("Categories")]
        [HttpGet("Categories")]
        public IActionResult Categories([FromQuery]bool? hasCupo = null)
        {            
            var res = _context.AlertCategories.Select(r => new { r.Id, r.Name, r.Icon }).ToList();

            if (res == null)
            {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            }
            else
            {
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = res });
            }
        }

        // GET: api/Alerts
        [HttpGet]
        public IActionResult GetAlerts([FromQuery] int? id = null, [FromQuery]DateTime? startDate = null, [FromQuery]DateTime? endDate = null,
            [FromQuery] int? userId = null, [FromQuery]int? buildingId = null,
            [FromQuery] int? categoryId = null, [FromQuery] int? statusId = null)
        {
            var res = _context.Alerts.Select(r => new {
                r.Id, r.Information, r.Photo, r.Description, r.AvailableSchedule, r.CreationDate, r.UserId, r.BuildingId, r.AlertCategoryId, r.AlertStatusId
            });

            if (id.HasValue) { res = res.Where(r => r.Id == id); }            
            if (startDate.HasValue) { res = res.Where(r => r.CreationDate > startDate); }
            if (endDate.HasValue) { res = res.Where(r => r.CreationDate < endDate); }
            if (userId.HasValue) { res = res.Where(r => r.UserId == userId); }
            if (buildingId.HasValue) { res = res.Where(r => r.BuildingId == buildingId); }
            if (categoryId.HasValue) { res = res.Where(r => r.AlertCategoryId == categoryId); }
            if (statusId.HasValue) { res = res.Where(r => r.AlertStatusId == statusId); }

            if (res == null) {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            }
            else {
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = res });
            }
        }

        ////LastAlerts
        ////Notifications (Slide Gesture) in the CLIENT
        //[Route("GetLastAlerts")]
        //[HttpGet("GetLastAlerts")]
        //public IActionResult GetLastAlerts([FromQuery] int userId, [FromQuery]DateTime? startDate = null, [FromQuery]DateTime? endDate = null)
        //{
        //    var res = _context.Alerts
        //        .Include(r => r.AlertCategory)
        //        .Include(r => r.AlertStatus)
        //        .Include(r => r.AlertMessages)
        //        .Where(r => r.UserId == userId);

        //    if (startDate.HasValue) { res = res.Where(r => r.CreationDate > startDate); }
        //    if (endDate.HasValue) { res = res.Where(r => r.CreationDate < endDate); }

        //    res = res.Where(r => r.AlertMessages.Count(x => x.Seen == false && x.UserId != userId) > 0);

        //    var response = res.Select(r => new {
        //        alertId = r.Id,
        //        r.AlertCategory.Name,
        //        r.AlertCategory.Icon,
        //        r.AlertMessages.OrderByDescending(x => x.Id).FirstOrDefault().Message,
        //        r.AlertMessages.OrderByDescending(x => x.Id).FirstOrDefault().Date,
        //    }).OrderByDescending(r => r.Date).Take(3);

        //    if (res == null)
        //    {
        //        return Ok(new { result = "Error", detalle = "Error", item = 0 });
        //    }
        //    else
        //    {
        //        return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = response });
        //    }
        //}

        //LastAlerts
        //Notifications (Slide Gesture) in the CLIENT
        [Route("GetLastAlerts")]
        [HttpGet("GetLastAlerts")]
        public IActionResult GetLastAlerts([FromQuery] int userId, [FromQuery]DateTime? startDate = null, [FromQuery]DateTime? endDate = null)
        {
            var res = _context.Alerts
                .Include(r => r.AlertCategory)
                .Include(r => r.AlertStatus)
                .Include(r => r.AlertMessages)
                .Where(r => r.UserId == userId);

            if (startDate.HasValue) { res = res.Where(r => r.CreationDate > startDate); }
            if (endDate.HasValue) { res = res.Where(r => r.CreationDate < endDate); }
            
            //res = res.Where(r => r.AlertMessages.Count(x => x.Seen == false && x.UserId != userId) > 0);

            var response = res.Select(r => new {
                alertId = r.Id,                                
                r.AlertCategory.Name,
                r.AlertCategory.Icon,
                r.AlertMessages.OrderByDescending(x => x.Id).FirstOrDefault().Message,
                r.AlertMessages.OrderByDescending(x => x.Id).FirstOrDefault().Date,
            }).OrderByDescending(r => r.Date).Take(3);

            if (res == null)
            {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            }
            else
            {
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = response });
            }
        }

        // ChatAlert View
        [Route("GetAlertsChat")]
        [HttpGet("GetAlertsChat")]
        public IActionResult GetAlertsChat([FromQuery] int userId, [FromQuery]DateTime? startDate = null, [FromQuery]DateTime? endDate = null)
        {
            var res = _context.Alerts
                .Include(r => r.AlertCategory)
                .Include(r => r.AlertStatus)
                .Include(r => r.AlertMessages)                
                .Where(r => r.UserId == userId);
            
            if (startDate.HasValue) { res = res.Where(r => r.CreationDate > startDate); }
            if (endDate.HasValue) { res = res.Where(r => r.CreationDate < endDate); }

            var response = res.Select( r=> new {
                r.Id, r.Information, r.AlertStatus.Color, r.AlertCategory.Name, r.AlertCategory.Icon,
                r.AlertMessages.OrderByDescending(x=>x.Id).FirstOrDefault().Message,
                r.AlertMessages.OrderByDescending(x => x.Id).FirstOrDefault().Date
            }).OrderByDescending(r => r.Date);

            if (res == null)
            {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            }
            else
            {
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = response });
            }
        }

        // Conversation View
        [Route("GetAlertTitle")]
        [HttpGet("GetAlertTitle")]
        public IActionResult GetAlertTitle([FromQuery] int alertId)
        {
            var res = _context.Alerts
                .Include(r => r.AlertCategory)                
                .Where(r => r.Id == alertId);            

            var response = res.Select(r => new {
                r.Id,
                r.Information,                
                r.AlertCategory.Name,
                r.AlertCategory.Icon                
            });

            if (res == null)
            {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            }
            else
            {
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = response.FirstOrDefault() });
            }
        }

        //Conversation View in the Client
        [Route("GetAlertsMessages")]
        [HttpGet("GetAlertsMessages")]
        public IActionResult GetAlertsMessages([FromQuery] int alertId, [FromQuery] int userId, [FromQuery]DateTime? startDate = null, [FromQuery]DateTime? endDate = null)
        {
            var U_session = (from a in _context.Users
                             where a.Id == userId
                             select new { id = a.Id }
                            ).ToList();

            if (U_session.Count() <= 0) { return Unauthorized(); }

            var res = _context.AlertMessages.Include(r => r.AlertStatus).Where(r => r.AlertId == alertId);
            if (startDate.HasValue) { res = res.Where(r => r.Date > startDate); }
            if (startDate.HasValue) { res = res.Where(r => r.Date < endDate); }

            foreach (AlertMessage m in res.ToList())
            {
                if (m.UserId == userId) { continue; }

                m.Seen = true;
                _context.AlertMessages.Update(m);
            }

            var response = res.Select(r => new {
                r.Id, r.Message, r.Date, r.UserId, statusAlert = new { r.AlertStatus.Id, r.AlertStatus.Status, r.AlertStatus.Color}
            });
            _context.SaveChanges();
            return Ok(new { result = "Success", detalle = "Consulta realizada con exito", item = response });
        }

        // AlertsNewForm and SecurityAlert in the Client
        // POST: api/Alerts        
        [HttpPost]
        public IActionResult NewAlert([FromBody] Alert alert)
        {
            
            try
            {
                alert.AlertStatusId = 1;    // id for the status Open
                _context.Alerts.Add(alert);
                _context.SaveChanges();

                // Automatic Message  for the alert
                AlertMessage alertMessage = new AlertMessage();
                alertMessage.AlertStatusId = alert.AlertStatusId;
                alertMessage.Message = alert.Information;
                alertMessage.AlertId = alert.Id;
                alertMessage.UserId = alert.UserId;
                _context.AlertMessages.Add(alertMessage);
                _context.SaveChanges();
            }
            catch(Exception e)
            {
                return Ok(new { result = "Error", detalle = "Error ocurren when creating the alert", item = 0 });
            }

            var response = new { alert.Id, alert.UserId, alertUserId = alert.UserId };
            _chatHubContext.Clients.All.SendAsync("Send", response);   // Send is the name thaw we're going to use in the client to bind the event                  
            return Ok(new { result = "Success", detalle = "Alert Created", item = new { alert.Id } });            
        }

        //Conversation View in the Client
        // POST: api/Alerts         
        [Route("SentMessage")]
        [HttpGet("SentMessage")]        
        public IActionResult SentMessage([FromBody] AlertMessage alertMessage)
        {
            Alert alert = _context.Alerts.FirstOrDefault(r => r.Id == alertMessage.AlertId);
            if (alert == null) { return NotFound(); }

            //////////////////////////// VALIDATION FOR ALERT IS CLOSED, CANT PUSH NEW MESSAGES ///////////////////////////////////////
            if (alert.AlertStatusId == 3) // 3 = ClosedStatus
            {
                return Ok(new { result = "Error", detalle = "Alert Closed", item = 0 });
            }

            //////////////////////////// VALIDATION AlERT IS OPEN, CANT PUSH NEW MESSAGES WITH OPENSTATUS ///////////////////////////////////////
            /// We can reject every AlertMessage with AlertStatus = 1 OpenStatus, 
            /// Because the ONLY way to create an AlertMessage with OpenStatus is througth NewAlert() EndPoint
            bool alreadyOpened = _context.AlertMessages.Count(r => r.AlertId == alertMessage.AlertId && r.AlertStatusId == 1) > 0;
            if (alertMessage.AlertStatusId == 1 && alreadyOpened ) { return Ok(new { result = "Error", detalle = "Alert Already Opened", item = 0 }); }

            try
            {                                                                
                _context.AlertMessages.Add(alertMessage);
                _context.SaveChanges();

                // Updating the status of the Alert with this new status of this message, ONLY IF IS NOT EMPTY STATUS
                // 0 = EmptyStatus
                if (alertMessage.AlertStatusId != 0)
                {                    
                    alert.AlertStatusId = alertMessage.AlertStatusId;
                    _context.Alerts.Update(alert);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return Ok(new { result = "Error", detalle = "Error ocurren when creating the alert", item = 0 });
            }

            var response = new { alertMessage.AlertId, alertMessage.UserId, alertUserId = alert.UserId };            
            _chatHubContext.Clients.All.SendAsync("Send", response);   // Send is the name thaw we're going to use in the client to bind the event           
            return Ok(new { result = "Success", detalle = "AlertMessage sent", item = response });
        }

    }
}


/*
 * SELECT SOME PROPERTIES IN LINQ 
 *  
 * www.brentozar.com/archive/2016/09/select-specific-columns-entity-framework-query/
 * stackoverflow.com/questions/36573224/how-to-only-select-specific-properties-from-an-object-graph-in-entity-framework
 * stackoverflow.com/questions/35308069/how-to-select-specific-properties-through-linq?lq=1
 * 
 */
