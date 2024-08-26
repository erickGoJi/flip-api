using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using flip.biz;
using flip.dal;
using flip.dal.DB_context;
using flip.biz.Entities;
using flip.api.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using flip.api.Services;
using System.IO;
using flip.api.Models.Email;

namespace flip.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecoverPass : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly Db_FlipContext _context;
        private IEmailRepository _emailRepository;

        public RecoverPass(IConfiguration config, Db_FlipContext context, IEmailRepository emailRepository)
        {
            _config = config;
            _context = context;
            _emailRepository = emailRepository;

        }
        public class mail
        {
            public string username { get; set; }
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values

        //POST API RECOVER PASSWORD 
        [Route("RecoverPassword")]
        [HttpPost]
        public ActionResult<ApiResponse<string>> RecoverPassword([FromBody] mail login)
        {

            var item = _context.Users.FirstOrDefault(t => t.Email == login.username);
            var result = new ApiResponse<string>();
            var response = new ApiResponse<string>();

            if (item == null)
            {
                result = new ApiResponse<string>()
                {
                    Success = false,
                    Result = " User Not found"

                };
            }
            else
            {
                var guid = Guid.NewGuid().ToString().Substring(0, 4);
                var path = Path.GetFullPath("TemplateMail/RecoverPass.html");
                StreamReader reader = new StreamReader(Path.GetFullPath("TemplateMail/RecoverPass.html"));
                string body = string.Empty;
                body = reader.ReadToEnd();
                body = body.Replace("{username}", "Hola," + " " + item.Name + " " + item.LastName + " " + item.MotherName);
                body = body.Replace("{pass}", "Tu nueva contraseña es:" + " " + "FLIP" + guid);
                try
                {

                    Email newmail = new Email();
                    newmail.To = login.username;
                    newmail.Subject = "Recuperar contraseña";
                    newmail.Body = body;
                    newmail.IsBodyHtml = true;
                    _emailRepository.SendEmail(newmail);
                    item.Password = "FLIP" + guid;
                    _context.SaveChanges();
                    result = new ApiResponse<string>()
                    {
                        Result = "La contraseña se cambio correctamente"
                    };
                }
                catch (Exception ex)
                {
                    result = new ApiResponse<string>()
                    {
                        Success = false ,
                        Result = ex.ToString()
                
                    };

                }
            }
            return new ObjectResult(result);

        }


        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
