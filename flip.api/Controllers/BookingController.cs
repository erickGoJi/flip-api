using System;
using System.Collections.Generic;
using System.Linq;
using flip.dal.DB_context;
using Microsoft.AspNetCore.Mvc;
using flip.api.Models;
using flip.biz.Entities;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace flip.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly Db_FlipContext _context;

        public BookingController(Db_FlipContext context)
        {
            _context = context;
        }
        public class ServicesModel
        {
            public int userid { get; set; }
            public int buildingid { get; set; }


        }

        public class ServicesCardModel
        {
            public int userid { get; set; }
            public int buildingid { get; set; }
            public int cardid { get; set; }


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

        /**CONSULTAR NewsFeed */
        [Route("SeeHistorical")]
        [HttpPost("{SeeHistorical}")]
        public IActionResult SeeHistorical([FromBody] ServicesModel item)
        {
            IActionResult response = Unauthorized();

            var U_session = (from a in _context.Users
                             where a.Id == item.userid
                             select new { id = a.Id }).ToList();

            var result = new ApiResponse<List<HistoricalRoom>>();

            var HistoricalBooking = (from c in _context.Bookings
                                     join u in _context.Users on c.IdUser equals u.Id
                                     join d in _context.Rooms on c.IdRoom equals d.Id
                                     join b in _context.Buildings on c.IdRoom equals b.Id

                                     where c.IdUser == item.userid 
                                     select new
                                     {
                                         buildingname = b.Name,
                                         booked = c.IdRoomNavigation.Name,
                                         bookeddetail = c.IdRoomNavigation.Description,
                                         idservicio = (Int32)c.Id,
                                         fecini = c.DateInitProgram.ToString(" MMMM yyyy"),
                                         fecexpect = c.DateInitReal,
                                         fecfinish = c.DateEndProgram,
                                         bookedservices = (from c1 in _context.ServiceBookings
                                                           join u in _context.Users on c.IdUser equals u.Id
                                                           where c1.IdBookingNavigation.IdUser == item.userid
                                                           select new
                                                           {
                                                               idservicio = (Int32)c.Id,
                                                               booked = c1.IdServiceNavigation.Name,
                                                               bookeddetail = c1.IdServiceNavigation.Description,
                                                               fecini = c1.DateStart.ToString("dddd, dd MMMM yyyy"),
                                                               fecexpect = c1.DateStart,
                                                               fecfinish = c1.DateEnd,

                                                           }).ToList(),
                                     }).ToList();
            var HistoricalRoom = (from c in _context.HistoricalRooms
                                  join u in _context.Users on c.UserId equals u.Id
                                  join s in _context.HistoricalServices on  c.UserId equals s.Id
                                  join b in _context.Buildings on c.RoomsId equals b.Id
                                  join bi in _context.Rooms on b.Id equals bi.BuildingId

                                  where c.UserId == item.userid
                                 
                                  select new
                                  {
                                      buildingname  = b.Name,
                                      idservicio = (Int32)c.Id,
                                      booked = c.Rooms.Name,
                                      bookeddetail = c.Rooms.Description,
                                      fecini = c.InitialDate.ToString(" MMMM yyyy"),
                                      fecexpect = c.ExpectedDate,
                                      fecfinish = c.FinishDate,

                                      bookedservices = (from c1 in _context.HistoricalServices
                                                        join u in _context.Users on c.UserId equals u.Id
                                                        where c1.UserId == item.userid 
                                                        select new
                                                        {
                                                            idservicio = (Int32)c.Id,
                                                            booked = c1.Services.Name,
                                                            bookeddetail = c1.Services.Description,
                                                            fecini = c1.InitialDate.ToString("dddd, dd MMMM yyyy"),
                                                            fecexpect = c1.ExpectedDate,
                                                            fecfinish = c1.FinishDate,

                                                        }).ToList(),
                                            }).ToList();



            if (HistoricalBooking == null)
            {
                /* result = new ApiResponse<List<Post>>()
                 {
                     Result = null, 
                     Success=false,
                     Message= "Can't get the comments "


                 };*/
                return response = Ok(new { result = "Error", detalle = "Error", item = 0 });

            }

            else {
                return response = Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = HistoricalBooking });
            }



        }


        /**CONSULTAR NewsFeed */
        [Route("SeeStateAccount")]
        [HttpPost("{SeeStateAccount}")]
        public IActionResult SeeStateAccount([FromBody] ServicesModel item)
        {
            IActionResult response = Unauthorized();

            var U_session = (from a in _context.Users
                             where a.Id == item.userid
                             select new { id = a.Id }).ToList();

            var result = new ApiResponse<List<HistoricalMembership>>();


            var HistoricalMember = (from hist in _context.MembershipBookings//MembershipBooking
                                    join c in _context.Bookings on hist.IdBooking equals c.Id//Bookings
                                    join u in _context.Users on c.IdUser equals u.Id

                                    join mem in _context.Memberships on hist.IdMembership equals mem.Id
                                    join bi in _context.Rooms on c.IdRoom equals bi.Id
                                    join b in _context.Buildings on c.IdRoom equals b.Id
                                    
                                    where c.IdUser == item.userid
                                    select new
                                    {
                                        membresia = mem.Name,
                                        memdesc = mem.Description,
                                        memprice = (hist.Payment == 0 || hist.Payment == null) ? mem.Price : 0,
                                        buildingname = b.Name,
                                        idservicio = (Int32)c.Id,
                                        booked = c.IdRoomNavigation.Name,
                                        bookeddetail = c.IdRoomNavigation.Description,
                                        fecini = c.DateInitProgram.ToString(" MMMM yyyy"),
                                        fecexpect = c.DateInitProgram.ToString(" MMMM yyyy"),
                                        fecfinish = c.DateEndProgram,
                                        idMembership = hist.Id,

                                        services = (from mi in _context.MembershipIndices
                                                    join s in _context.Services on mi.ServiceId equals s.Id
                                                    where mi.MembershipId == hist.IdMembership && hist.Payment == 0
                                                    select new {
                                                        s.Name
                                                    }).ToList(),


                                        bookedservices = (from c1 in _context.ServiceBookings
                                                          join u in _context.Users on c.IdUser equals u.Id
                                                          join p in _context.UserPaymentServices on c1.IdUserPaymentService equals p.Id
                                                          where c1.IdBookingNavigation.IdUser == item.userid && p.Payment == 0
                                                          select new
                                                          {
                                                              idServiceBooking = c1.Id,
                                                              idservicio = (Int32)c.Id,
                                                              booked = c1.IdServiceNavigation.Name,
                                                              bookeddetail = c1.IdServiceNavigation.Description,
                                                              fecini = c1.DateStart.ToString("dddd, dd MMMM yyyy"),
                                                              fecexpect = c1.DateStart,
                                                              fecfinish = c1.DateEnd,
                                                              cost = c1.IdServiceNavigation.Price, 

                                                          }).ToList(),
                                    }).ToList();

            if (HistoricalMember == null || HistoricalMember.Count == 0)
            {
                return response = Ok(new { result = "Error", detalle = "Error", item = 0 });
            }
            else
            {
                return response = Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = HistoricalMember });
            }
        }

        [Route("UpdateBookingMembership")]
        [HttpPost("{UpdateBookingMembership}")]
        public IActionResult UpdateBookingMembership ([FromBody] Booking booking)
        {
            IActionResult response = Unauthorized();
            try
            {
                var book = _context.Bookings.FirstOrDefault(s => s.Id == booking.Id);

                var serviceBooking = _context.ServiceBookings.Include(x => x.IdUserPaymentServiceNavigation)
                    .Where(a => a.IdBooking == booking.Id).ToList();
                foreach (ServiceBooking b in serviceBooking)
                {
                    b.IdUserPaymentServiceNavigation.Payment = 1;
                    b.IdUserPaymentServiceNavigation.PaymentDate = DateTime.Today;
                    book.ServiceBookings.Add(b);
                }
                var membership = _context.MembershipBookings.FirstOrDefault(m => m.IdBooking == booking.Id);
                membership.Payment = 1;
                membership.PaymentDate = DateTime.Today;
                book.MembershipBookings.Add(membership);
                _context.Bookings.Update(book);
                _context.SaveChanges();
                return Ok(new { result = "Success", detalle = "Adición realizada con éxito", item = 0 });   
            }
            catch (Exception ex)
            {
                return response = Ok(new { result = "Error", detalle = "Error al realizar la actualización", item = ex });
            }
            
        }

        [Route("Payment")]
        [HttpGet("Payment/{token}/{amount}")]
        public IActionResult procedPayment (string token, int amount)
        {
            IActionResult response = Unauthorized();
            try
            {
                StripeConfiguration.ApiKey = "sk_test_d2BxL70HP7z4tkVLuJuLIO3N00NWzfvcph";

                var options = new ChargeCreateOptions
                {
                    Amount = 1000,
                    Currency = "mxn",
                    Description = "Example charge",
                    Source = token,
                };
                var service = new ChargeService();
                Charge charge = service.Create(options);
                return response = Ok(new { result = "Success", detalle = "Pagó prcesado con exito.", item = charge });
            } catch (StripeException e )
            {
                return response = Ok(new { result = "Error", detalle = "Error al procesar el pagó.", item = e.Message });
            }
            
        }

        /**CONSULTAR NewsFeed */
        [Route("SeeCreditCard")]
        [HttpPost("{SeeCreditCard}")]
        public IActionResult SeeCreditCard ([FromBody] ServicesModel item)
        {
            IActionResult response = Unauthorized();

            var U_session = (from a in _context.Users
                             where a.Id == item.userid
                             select new { id = a.Id }).ToList();

            var result = new ApiResponse<List<CreditCard>>();
            var CreditCard = (from cc in _context.CreditCards
                                    join u in _context.Users on cc.UserId equals u.Id
                              
                                    where cc.UserId == item.userid
                                    select new
                                    {
                                        id= cc.Id,
                                        name= cc.Name,
                                        number= cc.Number, 
                                        month = cc.Month, 
                                        year = cc.Year

                                    }).ToList();
            

            if (CreditCard == null)
            {
               
                return response = Ok(new { result = "Error", detalle = "Error", item = 0 });

            }

            else
            {
                return response = Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = CreditCard });
            }



        }

        [Route("GetMainCreditCard")]
        [HttpGet("GetMainCreditCard/{idUser}")]
        public IActionResult GetMainCreditCard(int idUser)
        {
            IActionResult response = Unauthorized();

            var U_session = (from a in _context.Users
                             where a.Id == idUser
                             select new { id = a.Id }).ToList();

            var result = new ApiResponse<List<CreditCard>>();
            var CreditCard = (from cc in _context.CreditCards
                              join u in _context.Users on cc.UserId equals u.Id

                              where cc.UserId == idUser && cc.Main == 1
                              select new
                              {
                                  id = cc.Id,
                                  name = cc.Name,
                                  number = cc.Number,
                                  month = cc.Month,
                                  year = cc.Year

                              }).ToList();


            if (CreditCard == null)
            {

                return response = Ok(new { result = "Error", detalle = "Error", item = 0 });

            }

            else
            {
                return response = Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = CreditCard });
            }
        }





        /**CONSULTAR NewsFeed */
        [Route("SeeOneCreditCard")]
        [HttpPost("{SeeOneCreditCard}")]
        public IActionResult SeeOneCreditCard([FromBody] ServicesCardModel item)
        {
            IActionResult response = Unauthorized();

            var U_session = (from a in _context.Users
                             where a.Id == item.userid
                             select new { id = a.Id }).ToList();

            var result = new ApiResponse<List<CreditCard>>();
            var CreditCard = (from cc in _context.CreditCards
                              join u in _context.Users on cc.UserId equals u.Id

                              where cc.UserId == item.userid && cc.Id == item.cardid
                              select new
                              {
                                  id = cc.Id,
                                  name = cc.Name,
                                  number = cc.Number,
                                  month = cc.Month,
                                  year = cc.Year

                              }).ToList();


            if (CreditCard.Count == 0)
            {

                return response = Ok(new { result = "Error", detalle = "Error", item = 0 });

            }

            else
            {
                return response = Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = CreditCard });
            }



        }

        [Route("CreateUpdateCreditCard")]
        [HttpPost("{CreateUpdateCreditCard}")]
        public ActionResult<ApiResponse<string>> CreateUpdateCreditCard([FromBody] CreditCard item)
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
                    _context.CreditCards.Add(item);
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
                    var cc = _context.CreditCards.FirstOrDefault(s => s.Id == item.Id);
                    if (cc == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        cc.Name = item.Name;
                        cc.Number = item.Number;
                        cc.Month = item.Month;
                        cc.Year = item.Year;
                        

                        _context.CreditCards.Update(cc);
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



        [Route("DeleteCreditCard")]
        [HttpPost("{DeleteCreditCard}")]
        public ActionResult<ApiResponse<string>> DeleteCreditCard([FromBody] CreditCard item)
        {

            var cc = _context.CreditCards.FirstOrDefault(s => s.Id == item.Id);
            var response = new ApiResponse<string>();
            var result = new ApiResponse<string>();

            try
            {
                if (cc != null) // Crea Registro 
                {
                    _context.CreditCards.Remove(cc);
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


        }

        [Route("GetSchedules")]
        [HttpGet]
        public IActionResult postScheduleById()
        {
            var res = _context.Schedules;            

            if (res == null) {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            } else{
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = res });
            }
        }

    }
}
