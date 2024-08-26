using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using flip.api.Models;
using flip.api.Models.Building;
using flip.biz.Entities;
using flip.dal.DB_context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace flip.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly Db_FlipContext _context;

        public BuildingController(IConfiguration config, Db_FlipContext context)
        {
            _config = config;
            _context = context;

        }

        public class BuildingsModel
        {
            public int userid { get; set; }

        }

        [Route("GetCoverImage/{buildingId}")]
        [HttpGet("GetCoverImage/{buildingId}")]
        public IActionResult GetCoverImage(int buildingId)
        {
            var building = (from bi in _context.BuildingIndices
                            join pb in _context.PhotoBuilds on bi.BuildingId equals pb.Id
                            join b in _context.Buildings on bi.BuildingId equals b.Id
                            where b.Id == buildingId
                            select new {name = b.Name, description = b.Description,  photoURL = pb.PhotoUrl}
                );

            if (building == null) { return NotFound(); }

            return Ok(new { result = "Success", detalle = "Bulding found", item = building.FirstOrDefault() });
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }



        [Route("SeeBuildingweb")]
        [HttpPost("{SeeBuildingweb}")]
        public IActionResult SeeBuildingweb([FromBody] BuildingsModel item)
        {
            IActionResult response = Unauthorized();

            var result = new ApiResponse<List<Post>>();


            var Buildings = from c in _context.CommunitiesIndices
                           
                            select new
                            {
                                idbuild = (Int32)c.Id,
                                name = c.Name,

                            };
            var entidades = Buildings;


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

        [Route("SeeBuilding")]
        [HttpPost("{SeeBuilding}")]
        public IActionResult SeeBuilding([FromBody] BuildingsModel item)
        {
            IActionResult response = Unauthorized();

            var U_session = (from a in _context.Users
                             where a.Id == item.userid
                             select new { id = a.Id }).ToList();
            var result = new ApiResponse<List<Post>>();


            //var Buildings = from c in _context.Buildings
            //            join u in _context.BuildingIndices on c.Id equals u.BuildingId
            //            join a in _context.PhotoBuilds on u.PhotoBuildId equals a.Id
            //            select new
            //            {
            //                idbuild = (Int32)c.Id,
            //                name = c.Name,
            //                direction = c.Direction,
            //                photo = a.PhotoUrl

            //            };
            var Buildings = _context.Buildings;
                           
                            //select new
                            //{
                            //    idbuild = (Int32)c.Id,
                            //    name = c.Name,
                            //    direction = c.Direction,
                            //    photo = a.PhotoUrl

                            //};
            var entidades = Buildings;


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

        [Route("SaveBuilding")]
        [HttpPost]
        public IActionResult SaveBuilding([FromBody] BuildingDTO buildingDto)
        {
            Building buildData = new Building();
            buildData.TypeRooms = buildingDto.TypeRoom;
            buildData.Direction = buildingDto.Direction;
            buildData.Description = buildingDto.Description;
            buildData.Name = buildingDto.Name;
            buildData.Status = true;
            buildData.Photo = buildingDto.Photo;
            _context.Add(buildData);
            _context.SaveChanges();
            //buildData.TypeRooms = new TypeRoom();
            //buildData.TypeRooms.
            return Ok();
        }

        [Route("GetRooms")]
        [HttpGet]
        public IActionResult GetRooms(int buildId)
        {
            try
            {
                List<Room> listRooms = new List<Room>();

                listRooms = _context.Rooms.Where(wr => wr.BuildingId == buildId).ToList();
                if (listRooms.Count > 0)
                {
                    return Ok(listRooms);
                }
                else
                {
                    object error;
                    
                    return BadRequest(true);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        [Route("GetTypeRoom")]
        [HttpGet]
        public IActionResult GetTypeRoom(int buildId)
        {
            try
            {
                //List<TypeRoom> listRooms = new List<TypeRoom>();

                var listRooms = _context.TypeRooms.Where(wr=>wr.IdBuild == buildId).ToList();
                if (listRooms.Count > 0)
                {
                    return Ok(listRooms);
                }
                else
                {
                    object error;

                    return BadRequest(true);
                }
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Route("SaveRooms")]
        [HttpPost]
        public IActionResult SaveRooms([FromBody] List<Room> roomDTO)
        {
            List<Room> listRooms = new List<Room>();
            listRooms = roomDTO;
            _context.Rooms.AddRange(listRooms);
            _context.SaveChanges();
            //buildData.TypeRooms = new TypeRoom();
            //buildData.TypeRooms.
            return Ok();
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
