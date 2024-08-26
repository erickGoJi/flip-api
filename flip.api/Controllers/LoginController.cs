using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using flip.api.Models;
using flip.api.Models.Email;
using flip.api.Services;
using flip.biz.Entities;
using flip.dal.DB_context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace flip.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly Db_FlipContext _context;

        public LoginController(IConfiguration config, Db_FlipContext context)
        {
            _config = config;
            _context = context;
        }

        public class LoginModel
        {
            public string username { get; set; }
            public string password { get; set; }
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
        [Route("CargarUsuario")]
        [HttpPost]
        public IActionResult CargarUsuario( LoginModel login)
        {
          //  LoginModel  login = new LoginModel(); 
            IActionResult response = Unauthorized();
            var user = Authenticate(login);

            if (user.Name != "none" && user.Email != "none")
            {
                var tokenString = BuildToken(user);
                Token item = new Token
                {
                    Token1 = tokenString,
                    UserId = user.Id,
                    DateAcess = DateTime.Now,
                    Active = true,

                };

                _context.Tokens.Add(item);
                _context.SaveChanges();

                response = Ok(new { token = tokenString, user });

            }
            else
            {
                if (user.Name == "none")
                {
                    response = Ok(new { token = "usuarios no existe" });
                }

                if (user.Email == "none")
                {
                    response = Ok(new { token = "password incorrecto" });
                }
            }

            return response;
        }

        private User Authenticate(LoginModel login)
        {
            User user = null;

            var item = (from a in _context.Users
                        where a.Email == login.username
                        select new
                        {
                            id = a.Id,
                            name = a.Name,
                            paterno = a.LastName,
                            materno = a.MotherName,
                            password = a.Password,
                            email = a.Email,
                            buildingid=a.BuildingId,
                            SystemTypeId = a.SystemTypeId,
                        }).ToList();
            if (item.Count == 0)
            {
                user = new User { Id = 0, Name = "none", LastName = "none", MotherName = "none", Email = "" };
            }
            else
            {
                if (login.username == item[0].email && login.password == item[0].password)
                {
                    user = new User
                    {
                        Id = item[0].id,

                        Name = item[0].name,
                        LastName = item[0].paterno,
                        MotherName = item[0].materno,
                        Email = item[0].email,
                        BuildingId = item[0].buildingid,
                        SystemTypeId = item[0].SystemTypeId,

                    };
                }
                else
                {
                    if (login.password != item[0].password)
                    {
                        user = new User { Name = "", LastName = "none", MotherName = "none", Email = "none", };
                    }
                }
            }

            return user;

        }



        private string BuildToken(User user)
        {
            // Leemos el secret_key desde nuestro appseting
            var secretKey = _config.GetValue<string>("SecretKey");
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Name),
                            new Claim(ClaimTypes.Email, user.Email),
                        }),
                // Nuestro token va a durar un día
                Expires = DateTime.UtcNow.AddDays(1),
                // Credenciales para generar el token usando nuestro secretykey y el algoritmo hash 256
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };


            var tokenHandler = new JwtSecurityTokenHandler();
            var createdToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(createdToken);
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
