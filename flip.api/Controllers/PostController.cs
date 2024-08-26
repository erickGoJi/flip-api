using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using flip.api.Models;
using flip.biz.Entities;
using flip.dal.DB_context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace flip.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly Db_FlipContext _context;

        public PostController(IConfiguration config, Db_FlipContext context)
        {
            _config = config;
            _context = context;

        }

        public class SeePostModel
        {
            public int userid { get; set; }
            public int buildingid { get; set; }


        }

        public class SeeCommentModel
        {
            public int userid { get; set; }
            public int idpost { get; set; }


        }
        public class addComment {
            public int id { get; set; }
            public int userid { get; set; }
            public int postid { get; set; }
            public string comment { get; set; }
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
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }



        /**CONSULTAR home general Index */
        [Route("SeeDesign")]
        [HttpPost("{SeeDesign}")]
        public IActionResult SeeDesign([FromBody] SeePostModel item)
        {
            IActionResult response = Unauthorized();

            var U_session = (from a in _context.Users
                             where a.Id == item.userid
                             select new { id = a.Id }).ToList();
            var result = new ApiResponse<List<Post>>();


            var posts = from c in _context.DesignWebs
                        select new
                        {
                            id = (Int32)c.Id,
                            frontphoto = c.Photo,
                            position = c.Position,
                            posdesc = c.PositionDescription,
                            desc = c.Description,

                        };
            var entidades = posts;



            if (entidades == null)
            {
               
                return response = Ok(new { result = "Error", detalle = "Error", item = 0 });

            }


            return response = Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = entidades });
          


        }


        [Route("UpdateDesign")]
        [HttpPost("{UpdateDesign}")]
        public ActionResult<ApiResponse<string>> UpdateDesign([FromBody] DesignWeb item)
        {

            var response = new ApiResponse<string>();
            var result = new ApiResponse<string>();

            try
            {
                if (item.Id == 0) // Crea Registro 
                {
                    _context.DesignWebs.Add(item);
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
                    var post = _context.DesignWebs.FirstOrDefault(s => s.Id == item.Id);
                    if (post == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        post.Photo = item.Photo;
                        post.Position = item.Position;
                        post.Description = item.Description;
                        post.PositionDescription = item.PositionDescription;



                        _context.DesignWebs.Update(post);
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

        /**CONSULTAR home general Index */
        [Route("SeeHomeGeneral")]
        [HttpPost("{SeeHomeGeneral}")]
        public IActionResult SeeHomeGeneral([FromBody] SeePostModel item)
        {
            IActionResult response = Unauthorized();

            var U_session = (from a in _context.Users
                             where a.Id == item.userid
                             select new { id = a.Id }).ToList();
            var result = new ApiResponse<List<Post>>();


            var posts = from c in _context.CommunitiesGeneralWebs
                        select new
                        {
                            id = (Int32)c.Id,
                            frontphoto = c.Photo,
                            desc = c.Description,
                            photomobile = c.PhotoMobile,

                        };
            var entidades = posts;



            if (entidades == null)
            {
                return response = Ok(new { result = "Error", detalle = "Error", item = 0 });

            }


            return response = Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = entidades });
           


        }

        [Route("UpdateHomeGeneral")]
        [HttpPost("{UpdateHomeGeneral}")]
        public ActionResult<ApiResponse<string>> UpdateHomeGeneral([FromBody] CommunitiesGeneralWeb item)
        {

            var response = new ApiResponse<string>();
            var result = new ApiResponse<string>();

            try
            {
                if (item.Id == 0) // Crea Registro 
                {
                    _context.CommunitiesGeneralWebs.Add(item);
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
                    var post = _context.CommunitiesGeneralWebs.FirstOrDefault(s => s.Id == item.Id);
                    if (post == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        post.Photo = item.Photo;
                        post.Description = item.Description;
                        post.PhotoMobile = item.PhotoMobile;


                        _context.CommunitiesGeneralWebs.Update(post);
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



        /**CONSULTAR home ammenities Index */
        [Route("SeeHomeAmmenities")]
        [HttpPost("{SeeHomeAmmenities}")]
        public IActionResult SeeHomeAmmenities([FromBody] SeePostModel item)
        {
            IActionResult response = Unauthorized();

            var U_session = (from a in _context.Users
                             where a.Id == item.userid
                             select new { id = a.Id }).ToList();
            var result = new ApiResponse<List<Post>>();


            var posts = from c in _context.CommunitiesAmmenitiesWebs
                        select new
                        {
                            id = (Int32)c.Id,
                            title= c.Title, 
                            frontphoto = c.Photo,
                            desc = c.Description,
                            photomobile = c.PhotoMobile,
                            icon=c.Icon,
                            icon2=c.Icon2,
                            build= c.PhotoBuild,
                            buildmobile= c.PhotoBuilMobile,

                        };
            var entidades = posts;



            if (entidades == null)
            {
                
                return response = Ok(new { result = "Error", detalle = "Error", item = 0 });

            }


            return response = Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = entidades });
           


        }

        [Route("UpdateHomeAmmenities")]
        [HttpPost("{UpdateHomeAmmenities}")]
        public ActionResult<ApiResponse<string>> UpdateHomeAmmenities([FromBody] CommunitiesAmmenitiesWeb item)
        {

            var response = new ApiResponse<string>();
            var result = new ApiResponse<string>();

            try
            {
                if (item.Id == 0) // Crea Registro 
                {
                    _context.CommunitiesAmmenitiesWebs.Add(item);
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
                    var post = _context.CommunitiesAmmenitiesWebs.FirstOrDefault(s => s.Id == item.Id);
                    if (post == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        post.Photo = item.Photo;
                        post.Description = item.Description;
                        post.PhotoMobile = item.PhotoMobile;
                        post.Icon = item.Icon;
                        post.Icon2 = item.Icon2;
                        post.PhotoBuild = item.PhotoBuild;
                        post.PhotoBuilMobile = item.PhotoBuilMobile;
                        post.Title = item.Title; 
                          

                        _context.CommunitiesAmmenitiesWebs.Update(post);
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






        /**CONSULTAR more Index */
        [Route("SeeHomeServicios")]
        [HttpPost("{SeeHomeServicios}")]
        public IActionResult SeeHomeServicios([FromBody] SeePostModel item)
        {
            IActionResult response = Unauthorized();

            var U_session = (from a in _context.Users
                             where a.Id == item.userid
                             select new { id = a.Id }).ToList();
            var result = new ApiResponse<List<Post>>();


            var posts = from c in _context.CommunitiesServiciosWebs
                        select new
                        {
                            id = (Int32)c.Id,
                            frontphoto = c.Photo,
                            photomobile = c.PhotoMobile,
                            title = c.Title,
                            icon = c.Icon,
                            icon2 = c.Icon2,
                            category = c.Category

                        };
            var entidades = posts;



            if (entidades == null)
            {
                /* result = new ApiResponse<List<Post>>()
                 {
                     Result = null, 
                     Success=false,
                     Message= "Can't get the comments "


                 };*/
                return response = Ok(new { result = "Error", detalle = "Error", item = 0 });

            }


            return response = Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = entidades });
            //  return response = Ok(new { result = "Success", item = entidades });
            /*  result = new ApiResponse<List<Post>>()
              {
                  Result = entidades.Cast<List<Post>>(),
                  Success = true,
                  Message = "Success",
              };
              return result;*/


        }

        [Route("UpdateHomeServicios")]
        [HttpPost("{UpdateHomeServicios}")]
        public ActionResult<ApiResponse<string>> UpdateHomeServicios([FromBody] CommunitiesServiciosWeb item)
        {

            var response = new ApiResponse<string>();
            var result = new ApiResponse<string>();

            try
            {
                if (item.Id == 0) // Crea Registro 
                {
                    _context.CommunitiesServiciosWebs.Add(item);
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
                    var post = _context.CommunitiesServiciosWebs.FirstOrDefault(s => s.Id == item.Id);
                    if (post == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        post.Photo = item.Photo;
                        post.PhotoMobile = item.PhotoMobile;
                        post.Icon = item.Icon;
                        post.Title = item.Title;
                        post.Icon2 = item.Icon2; 
                        post.Category = item.Category; 

                        

                        _context.CommunitiesServiciosWebs.Update(post);
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

        /**CONSULTAR more Index */
        [Route("SeeHomeRoom")]
        [HttpPost("{SeeHomeRoom}")]
        public IActionResult SeeHomeRoom([FromBody] SeePostModel item)
        {
            IActionResult response = Unauthorized();

            var U_session = (from a in _context.Users
                             where a.Id == item.userid
                             select new { id = a.Id }).ToList();
            var result = new ApiResponse<List<Post>>();


            var posts = from c in _context.CommunitiesRoomWebs
                        where c.CommunitiesIndexId == 1
                        select new
                        {
                            id = (Int32)c.Id,
                            price = c.Price,
                            desc = c.Description,
                            title = c.Title,
                            photos = c.CommunitiesPhotosRoomWebs,
                        };
            var entidades = posts;



            if (entidades == null)
            {
                /* result = new ApiResponse<List<Post>>()
                 {
                     Result = null, 
                     Success=false,
                     Message= "Can't get the comments "


                 };*/
                return response = Ok(new { result = "Error", detalle = "Error", item = 0 });

            }


            return response = Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = entidades });
            //  return response = Ok(new { result = "Success", item = entidades });
            /*  result = new ApiResponse<List<Post>>()
              {
                  Result = entidades.Cast<List<Post>>(),
                  Success = true,
                  Message = "Success",
              };
              return result;*/


        }

        [Route("UpdateHomeRooms")]
        [HttpPost("{UpdateHomeRooms}")]
        public ActionResult<ApiResponse<string>> UpdateHomeRooms([FromBody] CommunitiesRoomWeb item)
        {

            var response = new ApiResponse<string>();
            var result = new ApiResponse<string>();

            try
            {
                if (item.Id == 0) // Crea Registro 
                {
                    _context.CommunitiesRoomWebs.Add(item);
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
                    var post = _context.CommunitiesRoomWebs.FirstOrDefault(s => s.Id == item.Id);
                    if (post == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        post.Description = item.Description;
                        post.Price = item.Price;
                        post.Title = item.Title;


                        _context.CommunitiesRoomWebs.Update(post);
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


        [Route("UpdateHomeRoomphoto")]
        [HttpPost("{UpdateHomeRoomphoto}")]
        public ActionResult<ApiResponse<string>> UpdateHomeRoomphoto([FromBody] CommunitiesPhotosRoomWeb item)
        {

            var response = new ApiResponse<string>();
            var result = new ApiResponse<string>();

            try
            {
                if (item.Id == 0) // Crea Registro 
                {
                    _context.CommunitiesPhotosRoomWebs.Add(item);
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
                    var post = _context.CommunitiesPhotosRoomWebs.FirstOrDefault(s => s.Id == item.Id);
                    if (post == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        post.Photo = item.Photo;
                        post.PhotoMobile = item.PhotoMobile;
                        post.Icon = item.Icon;
                        post.Icon2 = item.Icon2;
                        post.IdCommunitiesRoomWeb = item.IdCommunitiesRoomWeb;


                        _context.CommunitiesPhotosRoomWebs.Update(post);
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





















        /**CONSULTAR more Index */
        [Route("SeeHomeIndex")]
        [HttpPost("{SeeHomeIndex}")]
        public IActionResult SeeHomeIndex([FromBody] SeePostModel item)
        {
            IActionResult response = Unauthorized();

            var U_session = (from a in _context.Users
                             where a.Id == item.userid
                             select new { id = a.Id }).ToList();
            var result = new ApiResponse<List<Post>>();


            var posts = from c in _context.CommunitiesIndexWebs
                        select new
                        {
                            id = (Int32)c.Id,
                            frontphoto = c.Photo,
                            position = c.Position,
                            title = c.Title,
                            direction = c.Direction,
                            iscomming = c.IsComming == null ? false : true

                        };
            var entidades = posts;



            if (entidades == null)
            {
                /* result = new ApiResponse<List<Post>>()
                 {
                     Result = null, 
                     Success=false,
                     Message= "Can't get the comments "


                 };*/
                return response = Ok(new { result = "Error", detalle = "Error", item = 0 });

            }


            return response = Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = entidades });
            //  return response = Ok(new { result = "Success", item = entidades });
            /*  result = new ApiResponse<List<Post>>()
              {
                  Result = entidades.Cast<List<Post>>(),
                  Success = true,
                  Message = "Success",
              };
              return result;*/


        }

        [Route("UpdateHomeIndex")]
        [HttpPost("{UpdateHomeIndex}")]
        public ActionResult<ApiResponse<string>> UpdateHomeIndex([FromBody] CommunitiesIndexWeb item)
        {

            var response = new ApiResponse<string>();
            var result = new ApiResponse<string>();

            try
            {
                if (item.Id == 0) // Crea Registro 
                {
                    _context.CommunitiesIndexWebs.Add(item);
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
                    var post = _context.CommunitiesIndexWebs.FirstOrDefault(s => s.Id == item.Id);
                    if (post == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        post.Photo = item.Photo;
                        post.Position = item.Position;
                        post.Title = item.Title;
                        post.Direction = item.Direction;
                        post.IsComming = item.IsComming;

                        _context.CommunitiesIndexWebs.Update(post);
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


        /**CONSULTAR more Index */
        [Route("SeeMoreIndex")]
        [HttpPost("{SeeMoreIndex}")]
        public IActionResult SeeMoreIndex([FromBody] SeePostModel item)
        {
            IActionResult response = Unauthorized();

            var U_session = (from a in _context.Users
                             where a.Id == item.userid
                             select new { id = a.Id }).ToList();
            var result = new ApiResponse<List<Post>>();


            var posts = from c in _context.MoreIndexWebs
                        select new
                        {
                            id = (Int32)c.Id,
                            frontphoto = c.Photo,
                            position = c.Position,

                        };
            var entidades = posts;



            if (entidades == null)
            {
                /* result = new ApiResponse<List<Post>>()
                 {
                     Result = null, 
                     Success=false,
                     Message= "Can't get the comments "


                 };*/
                return response = Ok(new { result = "Error", detalle = "Error", item = 0 });

            }


            return response = Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = entidades });
            //  return response = Ok(new { result = "Success", item = entidades });
            /*  result = new ApiResponse<List<Post>>()
              {
                  Result = entidades.Cast<List<Post>>(),
                  Success = true,
                  Message = "Success",
              };
              return result;*/


        }

        [Route("UpdateMoreIndex")]
        [HttpPost("{UpdateMoreIndex}")]
        public ActionResult<ApiResponse<string>> UpdateMoreIndex([FromBody] MoreIndexWeb item)
        {

            var response = new ApiResponse<string>();
            var result = new ApiResponse<string>();

            try
            {
                if (item.Id == 0) // Crea Registro 
                {
                    _context.MoreIndexWebs.Add(item);
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
                    var post = _context.MoreIndexWebs.FirstOrDefault(s => s.Id == item.Id);
                    if (post == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        post.Photo = item.Photo;
                        post.Position = item.Position;


                        _context.MoreIndexWebs.Update(post);
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

        /**See social Networks */
        [Route("SeeSocialNetworks")]
        [HttpPost("{SeeSocialNetworks}")]
        public IActionResult SeeSocialNetworks([FromBody] SeePostModel item)
        {
            IActionResult response = Unauthorized();

            var U_session = (from a in _context.Users
                             where a.Id == item.userid
                             select new { id = a.Id }).ToList();
            var result = new ApiResponse<List<Post>>();


            var posts = from c in _context.SocialNetworksWebs
                        select new
                        {
                            id = (Int32)c.Id,
                            fb = c.FacebookUrl,
                            insta = c.InstagramUrl,
                            tt = c.TwitterUrl,
                            yt= c.YoutubeUrl,

                        };
            var entidades = posts;



            if (entidades == null)
            {
                /* result = new ApiResponse<List<Post>>()
                 {
                     Result = null, 
                     Success=false,
                     Message= "Can't get the comments "


                 };*/
                return response = Ok(new { result = "Error", detalle = "Error", item = 0 });

            }


            return response = Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = entidades });
            //  return response = Ok(new { result = "Success", item = entidades });
            /*  result = new ApiResponse<List<Post>>()
              {
                  Result = entidades.Cast<List<Post>>(),
                  Success = true,
                  Message = "Success",
              };
              return result;*/


        }

        [Route("UpdateSocialNetworks")]
        [HttpPost("{UpdateSocialNetworks}")]
        public ActionResult<ApiResponse<string>> UpdateSocialNetworks([FromBody] SocialNetworksWeb item)
        {

            var response = new ApiResponse<string>();
            var result = new ApiResponse<string>();

            try
            {
                if (item.Id == 0) // Crea Registro 
                {
                    _context.SocialNetworksWebs.Add(item);
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
                    var post = _context.SocialNetworksWebs.FirstOrDefault(s => s.Id == item.Id);
                    if (post == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        post.FacebookUrl = item.FacebookUrl;
                        post.TwitterUrl = item.TwitterUrl;
                        post.InstagramUrl = item.InstagramUrl;
                        post.YoutubeUrl = item.YoutubeUrl;


                        _context.SocialNetworksWebs.Update(post);
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



     



        /**CONSULTAR  design Index */
        [Route("SeeDesignIndex")]
        [HttpPost("{SeeDesignIndex}")]
        public IActionResult SeeDesignIndex([FromBody] SeePostModel item)
        {
            IActionResult response = Unauthorized();

            var U_session = (from a in _context.Users
                             where a.Id == item.userid
                             select new { id = a.Id }).ToList();
            var result = new ApiResponse<List<Post>>();


            var posts = from c in _context.DesignIndexWebs
                        select new
                        {
                            id = (Int32)c.Id,
                            frontphoto = c.Photo,
                            position = c.Position,

                        };
            var entidades = posts;



            if (entidades == null)
            {
                /* result = new ApiResponse<List<Post>>()
                 {
                     Result = null, 
                     Success=false,
                     Message= "Can't get the comments "


                 };*/
                return response = Ok(new { result = "Error", detalle = "Error", item = 0 });

            }


            return response = Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = entidades });
            //  return response = Ok(new { result = "Success", item = entidades });
            /*  result = new ApiResponse<List<Post>>()
              {
                  Result = entidades.Cast<List<Post>>(),
                  Success = true,
                  Message = "Success",
              };
              return result;*/


        }



        [Route("UpdateDesignIndex")]
        [HttpPost("{UpdateDesignIndex}")]
        public ActionResult<ApiResponse<string>> UpdateDesignIndex([FromBody] DesignIndexWeb item)
        {

            var response = new ApiResponse<string>();
            var result = new ApiResponse<string>();

            try
            {
                if (item.Id == 0) // Crea Registro 
                {
                    _context.DesignIndexWebs.Add(item);
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
                    var post = _context.DesignIndexWebs.FirstOrDefault(s => s.Id == item.Id);
                    if (post == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        post.Photo = item.Photo;
                        post.Position = item.Position;


                        _context.DesignIndexWebs.Update(post);
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



        /**CONSULTAR Index */
        [Route("SeeIndex")]
        [HttpPost("{SeeIndex}")]
        public IActionResult SeeIndex([FromBody] SeePostModel item)
        {
            IActionResult response = Unauthorized();

            var U_session = (from a in _context.Users
                             where a.Id == item.userid
                             select new { id = a.Id }).ToList();
            var result = new ApiResponse<List<Post>>();


            var posts = from c in _context.IndexWebPhotos
                        select new
                        {
                            id = (Int32)c.Id,
                            frontphoto = c.FrontPhoto,
                            backphoto = c.BackPhoto,

                        };
            var entidades = posts;



            if (entidades == null)
            {
                /* result = new ApiResponse<List<Post>>()
                 {
                     Result = null, 
                     Success=false,
                     Message= "Can't get the comments "


                 };*/
                return response = Ok(new { result = "Error", detalle = "Error", item = 0 });

            }


            return response = Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = entidades });
            //  return response = Ok(new { result = "Success", item = entidades });
            /*  result = new ApiResponse<List<Post>>()
              {
                  Result = entidades.Cast<List<Post>>(),
                  Success = true,
                  Message = "Success",
              };
              return result;*/


        }



        [Route("UpdateIndex")]
        [HttpPost("{UpdateIndex}")]
        public ActionResult<ApiResponse<string>> UpdateIndex([FromBody] IndexWebPhoto item)
        {

            var response = new ApiResponse<string>();
            var result = new ApiResponse<string>();

            try
            {
                if (item.Id == 0) // Crea Registro 
                {
                    _context.IndexWebPhotos.Add(item);
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
                    var post = _context.IndexWebPhotos.FirstOrDefault(s => s.Id == item.Id);
                    if (post == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        post.BackPhoto = item.BackPhoto;
                        post.FrontPhoto = item.FrontPhoto;


                        _context.IndexWebPhotos.Update(post);
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



        /**CONSULTAR Index */
        [Route("SeeNews")]
        [HttpPost("{SeeNews}")]
        public IActionResult SeeNews([FromBody] SeePostModel item)
        {
            IActionResult response = Unauthorized();

            var U_session = (from a in _context.Users
                             where a.Id == item.userid
                             select new { id = a.Id }).ToList();
            var result = new ApiResponse<List<Post>>();


            var posts = from c in _context.NewsWebs
                        select new
                        {
                            id = (Int32)c.Id,
                            photo = c.Photo,
                            title = c.Tittle,
                            reusme = c.Resume,
                            longresume = c.LongResumen,

                        };
            var entidades = posts;



            if (entidades == null)
            {
                /* result = new ApiResponse<List<Post>>()
                 {
                     Result = null, 
                     Success=false,
                     Message= "Can't get the comments "


                 };*/
                return response = Ok(new { result = "Error", detalle = "Error", item = 0 });

            }


            return response = Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = entidades });
            //  return response = Ok(new { result = "Success", item = entidades });
            /*  result = new ApiResponse<List<Post>>()
              {
                  Result = entidades.Cast<List<Post>>(),
                  Success = true,
                  Message = "Success",
              };
              return result;*/


        }



        [Route("UpdateNews")]
        [HttpPost("{UpdateNews}")]
        public ActionResult<ApiResponse<string>> UpdateNews([FromBody] NewsWeb item)
        {

            var response = new ApiResponse<string>();
            var result = new ApiResponse<string>();

            try
            {
                if (item.Id == 0) // Crea Registro 
                {
                    _context.NewsWebs.Add(item);
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
                    var post = _context.NewsWebs.FirstOrDefault(s => s.Id == item.Id);
                    if (post == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        post.Photo = item.Photo;
                        post.Resume = item.Resume;
                        post.LongResumen = item.LongResumen;
                        post.Tittle = item.Tittle;


                        _context.NewsWebs.Update(post);
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

        /**CONSULTAR NewsFeed */
        [Route("SeeTeam")]
        [HttpPost("{SeeTeam}")]
        public IActionResult SeeTeam([FromBody] SeePostModel item)
        {
            IActionResult response = Unauthorized();

            var U_session = (from a in _context.Users
                             where a.Id == item.userid
                             select new { id = a.Id }).ToList();
            var result = new ApiResponse<List<Post>>();


            var posts = from c in _context.TeamWebs
                        select new
                        {
                            id = (Int32)c.Id,
                            frontphoto = c.FrontPhoto,
                            backphoto = c.BackPhoto,
                            name = c.Name,
                            lastname = c.LastName,
                            position = c.Position,
                            desc = c.Description, 
                            link = c.LinkedinUrl, 
                            twitter=c.TwitterUrl, 

                        };
            var entidades = posts;



            if (entidades == null)
            {
                /* result = new ApiResponse<List<Post>>()
                 {
                     Result = null, 
                     Success=false,
                     Message= "Can't get the comments "


                 };*/
                return response = Ok(new { result = "Error", detalle = "Error", item = 0 });

            }


            return response = Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = entidades });
            //  return response = Ok(new { result = "Success", item = entidades });
            /*  result = new ApiResponse<List<Post>>()
              {
                  Result = entidades.Cast<List<Post>>(),
                  Success = true,
                  Message = "Success",
              };
              return result;*/


        }



        [Route("UpdateTeam")]
        [HttpPost("{UpdateTeam}")]
        public ActionResult<ApiResponse<string>> UpdateTeam([FromBody] TeamWeb item)
        {

            var response = new ApiResponse<string>();
            var result = new ApiResponse<string>();

            try
            {
                if (item.Id == 0) // Crea Registro 
                {
                    _context.TeamWebs.Add(item);
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
                    var post = _context.TeamWebs.FirstOrDefault(s => s.Id == item.Id);
                    if (post == null)
                    {
                        return BadRequest();
                    }
                    else
                    {


                        post.Name = item.Name;
                        post.LastName = item.LastName;
                        post.Description = item.Description;
                        post.Position = item.Position;
                        post.LinkedinUrl = item.LinkedinUrl;
                        post.TwitterUrl = item.TwitterUrl; 
                        post.BackPhoto = item.BackPhoto;
                        post.FrontPhoto = item.FrontPhoto;


                        _context.TeamWebs.Update(post);
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
        /**CONSULTAR NewsFeed */
        /**CONSULTAR NewsFeed */
        [Route("SeeJobs")]
        [HttpPost("{SeeJobs}")]
        public IActionResult SeeJobs([FromBody] SeePostModel item)
        {
            IActionResult response = Unauthorized();

            var U_session = (from a in _context.Users
                             where a.Id == item.userid
                             select new { id = a.Id }).ToList();
            var result = new ApiResponse<List<Post>>();


            var posts = from c in _context.JobWebs
                        select new
                        {
                            id = (Int32)c.Id,
                            title = c.Title,
                            longdesc = c.LongDescription,
                            shortdesc = c.ShortDescription,

                        };
            var entidades = posts;



            if (entidades == null)
            {
                /* result = new ApiResponse<List<Post>>()
                 {
                     Result = null, 
                     Success=false,
                     Message= "Can't get the comments "


                 };*/
                return response = Ok(new { result = "Error", detalle = "Error", item = 0 });

            }


            return response = Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = entidades });
            //  return response = Ok(new { result = "Success", item = entidades });
            /*  result = new ApiResponse<List<Post>>()
              {
                  Result = entidades.Cast<List<Post>>(),
                  Success = true,
                  Message = "Success",
              };
              return result;*/


        }



        [Route("UpdateJobs")]
        [HttpPost("{UpdateJobs}")]
        public ActionResult<ApiResponse<string>> UpdateJobs([FromBody] JobWeb item)
        {

            var response = new ApiResponse<string>();
            var result = new ApiResponse<string>();

            try
            {
                if (item.Id == 0) // Crea Registro 
                {
                    _context.JobWebs.Add(item);
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
                    var post = _context.JobWebs.FirstOrDefault(s => s.Id == item.Id);
                    if (post == null)
                    {
                        return BadRequest();
                    }
                    else
                    {


                        post.Title = item.Title;
                        post.ShortDescription = item.ShortDescription;
                        post.LongDescription = item.LongDescription;


                        _context.JobWebs.Update(post);
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
        /**CONSULTAR NewsFeed */






        [Route("SeePost")]
        [HttpPost("{SeePost}")]
        public IActionResult SeePost([FromBody] SeePostModel item)
        {
            IActionResult response = Unauthorized();

            var U_session = (from a in _context.Users
                             where a.Id == item.userid
                             select new { id = a.Id }).ToList();
            var result = new ApiResponse<List<Post>>();
            
            
               var posts  = from c in _context.Posts
                                 join u in _context.Users on c.UserId equals u.Id
                            where c.BuildingId  == item.buildingid
                                 select new
                                 {
                                     idpost = (Int32)c.Id,
                                     posttext = c.PostText,
                                     posttitle= c.Title,
                                     photo = c.Photo,
                                     usrwritter = c.UserId, 
                                     idbuilding=c.BuildingId,

                                 };
                var entidades = posts;



            if (entidades == null)
                {
                /* result = new ApiResponse<List<Post>>()
                 {
                     Result = null, 
                     Success=false,
                     Message= "Can't get the comments "


                 };*/
                return response = Ok(new { result = "Error", detalle = "Error", item = 0 });

            }


            return response = Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item=  entidades});
            //  return response = Ok(new { result = "Success", item = entidades });
            /*  result = new ApiResponse<List<Post>>()
              {
                  Result = entidades.Cast<List<Post>>(),
                  Success = true,
                  Message = "Success",
              };
              return result;*/


        }




        [Route("SeeOnePost")]
        [HttpPost("{SeeOnePost}")]
        public IActionResult SeeOnePost([FromBody] SeeCommentModel item)
        {
            IActionResult response = Unauthorized();

            var U_session = (from a in _context.Users
                             where a.Id == item.userid
                             select new { id = a.Id }).ToList();
            var result = new ApiResponse<List<Post>>();


            var posts = from c in _context.Posts
                        where c.Id == item.idpost
                        select new
                        {
                            idpost = (Int32)c.Id,
                            posttext = c.PostText,
                            posttitle = c.Title,
                            postphoto = c.Photo,
                            usrwritter = c.UserId,
                            idbuilding = c.BuildingId

                        };
            var entidades = posts;



            if (entidades == null)
            {
                /* result = new ApiResponse<List<Post>>()
                 {
                     Result = null, 
                     Success=false,
                     Message= "Can't get the comments "


                 };*/
                return response = Ok(new { result = "Error", detalle = "Error", item = 0 });

            }


            return response = Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = entidades });
            //  return response = Ok(new { result = "Success", item = entidades });
            /*  result = new ApiResponse<List<Post>>()
              {
                  Result = entidades.Cast<List<Post>>(),
                  Success = true,
                  Message = "Success",
              };
              return result;*/


        }
        [Route("SeeComment")]
        [HttpPost("{SeeComment}")]
        public IActionResult SeeComment([FromBody]SeeCommentModel item)
        {
            IActionResult response = Unauthorized();

            var U_session = (from a in _context.Users
                             where a.Id == item.userid
                             select new { id = a.Id }).ToList();
            var result = new ApiResponse<List<Post>>();


            var posts =( from c in _context.Comments
                        join a in _context.Posts on item.idpost equals a.Id
                        join u in _context.Users on item.userid equals u.Id
                        where c.PostId == item.idpost
                        select new
                        {
                            idpost = (Int32)c.Id,
                            comment = c.Comment1,
                            postid = c.PostId,

                            userid = c.UserId, 
                            name = (from c1 in _context.Users
                                    where c1.Id == c.UserId
                                    select new
                                    {
                                        c1.Name,
                                        c1.LastName,
                                    }).ToList(),

                        }).ToList();
            //_context.Users.FirstOrDefault(s => s.Id == ),
            var entidades = posts;


            if (entidades.Count==0)
            {
                /* result = new ApiResponse<List<Post>>()
                 {
                     Result = null, 
                     Success=false,
                     Message= "Can't get the comments "


                 };*/
                return response = Ok(new { result = "Error", detalle = "Error", item = 0 });

            }


            return response = Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = entidades });
            //  return response = Ok(new { result = "Success", item = entidades });
            /*  result = new ApiResponse<List<Post>>()
              {
                  Result = entidades.Cast<List<Post>>(),
                  Success = true,
                  Message = "Success",
              };
              return result;*/


        }

        [Route("PostComment")]
        [HttpPost("{PostComment}")]
        public ActionResult<ApiResponse<string>> PostComment([FromBody] Comment item)
        {
            var U_session = (from a in _context.Users
                             where a.Id == item.UserId
                             select new { id = a.Id }).ToList();
           
            var response = new ApiResponse<string>();
            var result = new ApiResponse<string>();

            try
            {
                if (item.Id == 0) // Crea Registro 
                {
                    _context.Comments.Add(item);
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
                else
                {
                    result = new ApiResponse<string>()
                    {
                        Result = "Error",
                        Success = false,
                        Message = "Can't post the comment",


                    };
                    return result;
                }
            }
            catch (Exception ex)
            {
                result = new ApiResponse<string>()
                {
                    Result = "Error",
                    Success = false,
                    Message = "Exception " +ex


                };
                return result;
            }
           


            

           
            //  return response = Ok(new { result = "Success", item = entidades });


        }

        [Route("DeleteComment")]
        [HttpPost("{DeleteComment}")]
        public ActionResult<ApiResponse<string>> DeleteComment([FromBody] Comment item)
        {
            var comment = _context.Comments.FirstOrDefault(s => s.Id == item.PostId);
            var response = new ApiResponse<string>();
            var result = new ApiResponse<string>();

            try
            {
                if (comment != null ) // Crea Registro 
                {
                    _context.Comments.Remove(comment);
                    _context.SaveChanges();
                    //return response = Ok(new { result = "Success", detalle = "Registro Creado con éxito.", id = item.Id });
                    result = new ApiResponse<string>()
                    {
                        Result = "Success",
                        Success = true,
                        Message = "Remove the comment",


                    };
                    return result;
                }
                else
                {
                    result = new ApiResponse<string>()
                    {
                        Result = "Error",
                        Success = false,
                        Message = "Can't remove the comment",


                    };
                    return result;
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






            //  return response = Ok(new { result = "Success", item = entidades });


        }

        [Route("DeletePost")]
        [HttpPost("{DeletePost}")]
        public ActionResult<ApiResponse<string>> DeletePost([FromBody] Post item)
        {

             var post = _context.Posts.FirstOrDefault(s => s.Id == item.Id);
            var comment = _context.Comments.Where(s => s.PostId == item.Id);
           /// IEnumerable<entity> list = db.entity.where(x => x.id == id).toList();
            // Use Remove Range function to delete all records at once
            //db.entity.RemoveRange(list);
            var response = new ApiResponse<string>();
            var result = new ApiResponse<string>();

            try
            {
                if ( post != null ) 
                {
                    if (comment == null)
                    {
                        _context.Posts.Remove(post);
                        _context.SaveChanges();
                    }
                    else
                    {
                        _context.Comments.RemoveRange(comment);
                        _context.Posts.Remove(post);
                        _context.SaveChanges();
                    }
                    //return response = Ok(new { result = "Success", detalle = "Registro Creado con éxito.", id = item.Id });
                    result = new ApiResponse<string>()
                    {
                        Result = "Success",
                        Success = true,
                        Message = "Remove the comment",


                    };
                    return result;
                }
                else
                {
                    result = new ApiResponse<string>()
                    {
                        Result = "Error",
                        Success = false,
                        Message = "Can't remove the Post",


                    };
                    return result;
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






            //  return response = Ok(new { result = "Success", item = entidades });


        }


        [Route("PostPosts")]
        [HttpPost("{PostPosts}")]
        public ActionResult<ApiResponse<string>> PostPosts([FromBody] Post item)
        {
            var U_session = (from a in _context.Users
                             where a.Id == item.UserId
                             select new { id = a.Id }).ToList();

            var response = new ApiResponse<string>();
            var result = new ApiResponse<string>();

            try
            {
                if (item.Id == 0) // Crea Registro 
                {
                    _context.Posts.Add(item);
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
                    var post = _context.Posts.FirstOrDefault(s => s.Id == item.Id);
                    if (post == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        post.Title = item.Title;
                        post.PostText= item.PostText;
                        post.Photo= item.Photo;
                        
                       
                        _context.Posts.Update(post);
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


      



         [Route("UploadImg")]
         [HttpPost("{UploadImg}")]
         public async Task<IActionResult> UploadImg(List<IFormFile> file)
         {
             var result = new ApiResponse<string>();

             try
             {

                 var selRuta = _context.FileParameters.FirstOrDefault(p => p.Id == 2);
                 string ruta;
                 if (selRuta == null) ruta = "Error";
                 else
                 {
                     long size = file.Sum(f => f.Length);

                     var filePath = Environment.CurrentDirectory;
                     var extencion = file[0].FileName.Split(".");
                     var _guid = Guid.NewGuid();
                     var path = selRuta.FileFolder + _guid + "." + extencion[1];

                     foreach (var formFile in file)
                     {
                         if (formFile.Length > 0)
                         {
                             using (var stream = new FileStream(filePath + path, FileMode.Create))
                             {
                                 await formFile.CopyToAsync(stream);
                             }
                         }
                     }
                     ruta = selRuta.Url + _guid + "." + extencion[1];
                 }


               
                 {
                     result.Result = "Sucess";
                     result.Success = true;
                     result.Message = ruta;
                 };
             }
             catch (Exception ex)
             {

                
                 {
                     result.Result = "Error";
                     result.Success = false;
                     result.Message = ex.ToString() ;
                 };

             }
            

            return new ObjectResult(result);
         }

        [Route("UploadImgBase64")]
        [HttpPost("{UploadImgBase64}")]
        public IActionResult UploadImgBase64([FromBody] string base64String)
        {
            var result = new ApiResponse<string>();

            try {
                var selRuta = _context.FileParameters.FirstOrDefault(p => p.Id == 2);
                string ruta;

                if (selRuta == null) { return Ok(new { result = "Error", detalle = "Ruta Not Found", item = 0 }); }
                
                var filePath = Environment.CurrentDirectory;                
                var extension = "jpg";
                var _guid = Guid.NewGuid();
                var path = selRuta.FileFolder + _guid + "." + extension;

                var bytes = Convert.FromBase64String(base64String);
                using (var imageFile = new FileStream(filePath + path, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }                

                ruta = selRuta.Url + _guid + "." + extension;
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = ruta });                
            } catch (Exception ex) {
                return Ok(new { result = "Error", detalle = ex.ToString(), item = 0 });                
            }           
        }


        [Route("UploadImgBase64Compress")]
        [HttpPost("UploadImgBase64Compress")]
        public IActionResult UploadImgBase64Compress([FromBody] string base64String)
        {
            var result = new ApiResponse<string>();

            try
            {
                var selRuta = _context.FileParameters.FirstOrDefault(p => p.Id == 2);
                string ruta;

                if (selRuta == null) { return Ok(new { result = "Error", detalle = "Ruta Not Found", item = 0 }); }

                var filePath = Environment.CurrentDirectory;
                var extension = "jpg";
                var _guid = Guid.NewGuid();
                var path = selRuta.FileFolder + _guid + "." + extension;

                var bytes = Convert.FromBase64String(base64String);
                using (var imageFile = new FileStream(filePath + path, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                    Models.Comun.ImageUtilities imageUtilities = new Models.Comun.ImageUtilities();
                    System.Drawing.Image image = imageUtilities.Redimensionar(imageFile);
                    image.Save(imageFile, System.Drawing.Imaging.ImageFormat.Jpeg);
                    imageFile.Flush();
                }

                ruta = selRuta.Url + _guid + "." + extension;
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = ruta });
            }
            catch (Exception ex)
            {
                return Ok(new { result = "Error", detalle = ex.ToString(), item = 0 });
            }
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
