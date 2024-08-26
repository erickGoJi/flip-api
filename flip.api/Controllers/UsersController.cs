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
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;

namespace flip.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Db_FlipContext _context;
        public UsersController(Db_FlipContext context)
        {
            _context = context;
        }
        public class SeePostModel
        {
            public int userid { get; set; }
            public int buildingid { get; set; }
        }

        //// GET api/values
        //[HttpGet]
        //public ActionResult<IEnumerable<string>> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public ActionResult <List<User>> Get(int id)
        //{
        //    var res = _context.Users.Where(a => a.Id == 1).ToList();
        //    return res;
        //}     

        // POST api/values
        [DisableCors]
        [HttpPost]
        public ActionResult<List<User>> Post([FromBody] string value)
        {
            var res = _context.Users.Where(a => a.Id == 1).ToList();
            return res;
        }



        [Route("get_usrs")]
        [HttpPost]
        public IActionResult get_usrs([FromBody]modstr value)
        {
            var res = _context.Users.Where(a => a.Id == 1).ToList();
            IActionResult response = Unauthorized();
            return response = Ok(new { result = "Okas", item = res });
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



        [Route("UpdateUser")]
        [HttpPost("{UpdateUser}")]
        public ActionResult<ApiResponse<string>> UpdateUser([FromBody] User item)
        {
            var U_session = (from a in _context.Users
                             where a.Id == item.Id
                             select new { id = a.Id }).ToList();

            var response = new ApiResponse<string>();
            var result = new ApiResponse<string>();

            try
            {
                if (item.Id == 0) // Crea Registro 
                {
                    _context.Users.Add(item);
                    _context.SaveChanges();
                    //return response = Ok(new { result = "Success", detalle = "Registro Creado con éxito.", id = item.Id });
                    result = new ApiResponse<string>()
                    {
                        Result = "Success",
                        Success = true,
                        Message = "Post the comment",


                    };
                    return result;
                }
                else // Edita Registo 
                {
                    var user = _context.Users.FirstOrDefault(s => s.Id == item.Id);
                    if (user == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        user.Name = item.Name;
                        user.LastName = item.LastName;
                        user.MotherName = item.MotherName;
                        user.Email = item.Email;
                        user.Password = item.Password;
                        user.Avatar = item.Avatar;
                        user.FacebookUrl = item.FacebookUrl;
                        user.TwitterUrl = item.TwitterUrl;
                        user.InstagramUrl = item.InstagramUrl;
                        user.Phone = item.Phone;
                        user.Workplace = item.Workplace;
                        user.AboutMe = item.AboutMe;
                        user.Modified = DateTime.Now;

                        _context.Users.Update(user);
                        _context.SaveChanges();
                        result = new ApiResponse<string>()
                        {
                            Result = "Success",
                            Success = false,
                            Message = "Registro actualizado con éxito ",


                        };
                        return result;
                    }
                }

            }
            catch (Exception ex)
            {
                result = new ApiResponse<string>()
                {
                    Result = "Error",
                    Success = false,
                    Message = "Exception " + ex


                };
                return result;
            }
        }

        [Route("UpdateToken")]
        [HttpPost("{UpdateToken}")]
        public ActionResult<ApiResponse<string>> UpdateToken([FromBody] User item)
        {
            var U_session = (from a in _context.Users
                             where a.Id == item.Id
                             select new { id = a.Id }).ToList();

            var response = new ApiResponse<string>();
            var result = new ApiResponse<string>();

            try
            {
                if (item.Id == 0) // Crea Registro 
                {
                    _context.Users.Add(item);
                    _context.SaveChanges();
                    //return response = Ok(new { result = "Success", detalle = "Registro Creado con éxito.", id = item.Id });
                    result = new ApiResponse<string>()
                    {
                        Result = "Success",
                        Success = true,
                        Message = "Post the comment",


                    };
                    return result;
                }
                else // Edita Registo 
                {
                    var user = _context.Users.FirstOrDefault(s => s.Id == item.Id);
                    if (user == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        user.Fcmtoken = item.Fcmtoken;

                        _context.Users.Update(user);
                        _context.SaveChanges();
                        result = new ApiResponse<string>()
                        {
                            Result = "Success",
                            Success = false,
                            Message = "Registro actualizado con éxito ",


                        };
                        return result;
                    }
                }

            }
            catch (Exception ex)
            {
                result = new ApiResponse<string>()
                {
                    Result = "Error",
                    Success = false,
                    Message = "Exception " + ex


                };
                return result;
            }
        }


        [Route("SeeFaqs")]
        [HttpPost("{SeeFaqs}")]
        public IActionResult SeeFaqs([FromBody] SeePostModel item)
        {
            IActionResult response = Unauthorized();

            var U_session = (from a in _context.Users
                             where a.Id == item.userid
                             select new { id = a.Id }).ToList();
            var result = new ApiResponse<List<Post>>();


            var faqs = from c in _context.Faqs

                       select new
                       {
                           id = (Int32)c.Id,
                           question = c.Question,
                           answer = c.Answer,
                       };
            var entidades = faqs;

            if (entidades == null) { return response = Ok(new { result = "Error", detalle = "Error", item = 0 }); }
            return response = Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = entidades });
            //  return response = ok(new { result = "success", item = entidades });
            /*  result = new apiresponse<list<post>>()
              {
                  result = entidades.cast<list<post>>(),
                  success = true,
                  message = "success",
              };
              return result;*/


        }



        [Route("SeeDetailUser")]
        [HttpPost("{SeeDetailUser}")]
        public IActionResult SeeDetailUser([FromBody] User item)
        {
            IActionResult response = Unauthorized();

            var U_session = (from a in _context.Users
                             where a.Id == item.Id
                             select new { id = a.Id }).ToList();
            var result = new ApiResponse<List<Post>>();


            var posts = from c in _context.Users
                        where c.Id == item.Id
                        select new
                        {
                            Name = c.Name,
                            LastName = c.LastName,
                            MotherName = c.MotherName,
                            Email = c.Email,
                            Password = c.Password,
                            Avatar = c.Avatar,
                            FacebookUrl = c.FacebookUrl,
                            TwitterUrl = c.TwitterUrl,
                            InstagramUrl = c.InstagramUrl,
                            Phone = c.Phone,
                            Workplace = c.Workplace,
                            AboutMe = c.AboutMe,
                            idbuilding = c.BuildingId

                        };

            var entidades = posts.ToList();
            if (entidades.Count() <= 0) { return response = Ok(new { result = "Error", detalle = "Error", item = 0 }); }
            return response = Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = entidades });
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////        
        // GET: api/Users
        [HttpGet]
        public IActionResult GetUsers([FromQuery] int? id = null, [FromQuery]int? buildingId = null, [FromQuery]DateTime? dateInit = null, [FromQuery]DateTime? dateEnd = null)
        {
            try
            {
                IQueryable<User> query = _context.Users;
                IQueryable<Booking> bookin = _context.Bookings.Include(i => i.IdUserNavigation);
                if (id.HasValue) { query = query.Where(a => a.Id == id); }
                if (buildingId.HasValue && dateInit == null)
                {
                    query = query.Where(a => a.BuildingId == buildingId);
                    return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = query });
                }
                if (buildingId.HasValue && dateInit.HasValue)
                {
                    DateTime endDate = Convert.ToDateTime(dateEnd);
                    query = query.Where(a => a.BuildingId == buildingId);
                    DateTime conv = Convert.ToDateTime(dateInit).Date;
                    endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 11, 59, 59);
                    query = bookin.Where(wr => conv < wr.DateInitProgram && wr.DateEndProgram > endDate && wr.IdUserNavigation.BuildingId == buildingId)
                            .Select(s => s.IdUserNavigation);
                    return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = query });
                }
                return BadRequest("Error al hacer la consulta");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public class modstr
        {
            private string str;
            public string Str { get => str; set => str = value; }
        }

    }
}
