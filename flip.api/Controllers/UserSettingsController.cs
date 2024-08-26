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
    public class UserSettingsController : ControllerBase
    {
        private readonly Db_FlipContext _context;

        public UserSettingsController(Db_FlipContext context){
            _context = context;
        }

        // GET: api/UserSettings
        [HttpGet]
        public IActionResult GetUserSettings([FromQuery] int? id = null, [FromQuery] string name = null, [FromQuery] int? userId = null)
        {
            var res = _context.UserSettings.Select(r => new {
                r.Id, r.Name, r.Value, r.UserId
            });

            if (id.HasValue) { res = res.Where(r => r.Id == id); }            
            if (userId.HasValue) { res = res.Where(r => r.UserId == userId); }
            if (name != null) { res = res.Where(r => r.Name == name); }

            if (res.Count() <= 0){
                return NotFound(new { result = "Error", detalle = "Error", item = 0 });
                //return Ok(new { result = "Error", detalle = "Error", item = 0 });
            }
            else{
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = res });
            }
        }
        
        // POST: api/UserSettings       
        [HttpPost]
        public IActionResult AddOrUpdateSettings([FromBody] UserSetting[] userSetting)
        {
            foreach (var setting in userSetting)
            {
                if (setting.Id == 0)
                {
                    var settingDetail = _context.UserSettings.FirstOrDefault(r => r.Name == setting.Name && r.UserId == setting.UserId);
                    if (settingDetail == null) {
                        _context.UserSettings.Add(setting);
                    } 
                } else
                {
                    var settingDetail = _context.UserSettings.Find(setting.Id);
                    settingDetail.Value = setting.Value;
                    _context.UserSettings.Update(settingDetail);
                }
            }

            try
            {               
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return Ok(new { result = "Error", detalle = "Error ocurren when creating the alert", item = 0 });
            }
            
            return Ok(new { result = "Success", detalle = "Settings Updated", item = userSetting });
        }

        // PUT: api/UserSettings/UpdateSetting
        [Route("UpdateSetting")]
        [HttpPut("UpdateSetting")]                
        public IActionResult UpdateSingleSetting([FromBody] UserSetting userSetting)
        {
            var settingDetail = _context.UserSettings.Find(userSetting.Id);
            settingDetail.Value = userSetting.Value;
            _context.UserSettings.Update(settingDetail);
                           
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return Ok(new { result = "Error", detalle = "Error ocurren when creating the alert", item = 0 });
            }

            return Ok(new { result = "Success", detalle = "Settings Updated", item = userSetting });
        }

        // DELETE: api/UserSettings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserSetting>> DeleteUserSetting(int id)
        {
            var userSetting = await _context.UserSettings.FindAsync(id);
            if (userSetting == null)
            {
                return NotFound();
            }

            _context.UserSettings.Remove(userSetting);
            await _context.SaveChangesAsync();

            return userSetting;
        }

        private bool UserSettingExists(int id)
        {
            return _context.UserSettings.Any(e => e.Id == id);
        }
    }
}
