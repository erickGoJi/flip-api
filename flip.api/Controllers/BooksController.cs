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
    public class BooksController : ControllerBase
    {
        private readonly Db_FlipContext _context;

        public BooksController(Db_FlipContext context)
        {
            _context = context;
        }

        //// GET: api/Books
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        //{
        //    return await _context.Books.ToListAsync();
        //}

        //// GET: api/Books/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Book>> GetBook(int id)
        //{
        //    var book = await _context.Books.FindAsync(id);

        //    if (book == null)
        //    {
        //        return NotFound();
        //    }

        //    return book;
        //}

        //// GET: api/Books
        [HttpGet]
        public IActionResult GetBooks([FromQuery] int? id = null, [FromQuery] int? scheduleId = null, [FromQuery] int? status = null, [FromQuery] int? userId = null, [FromQuery] int? activityId = null)
        {           
            var res = (from bk in _context.Books.Include(a => a.User).Include(a => a.Schedule)
                       join sc in _context.Schedules on bk.ScheduleId equals sc.Id
                       select new
                       {
                           bk.Id, bk.ScheduleId, bk.TimeStamp, bk.Status, bk.User, bk.Schedule
                       }
            );

            if (id.HasValue) { res = res.Where(r => r.Id == id); }
            if (scheduleId.HasValue) { res = res.Where(r => r.ScheduleId == scheduleId); }
            if (status.HasValue) { res = res.Where(r => r.Status == status); }
            if (userId.HasValue) { res = res.Where(r => r.User.Id == userId); }
            if (activityId.HasValue) { res = res.Where(a=> a.Schedule.ActivityId == activityId); }
            
            var res1 = (from result in res select new { result.Id, result.ScheduleId, result.TimeStamp, result.Status, result.User });
                        
            if (res == null) {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            }
            else {
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = res1 });
            }
        }

        // POST: api/Books
        //[HttpPost]
        //public IActionResult PostBook(Book book)
        //{
        //    if (_context.Books.Any(e => e.UserId == book.UserId && e.ScheduleId == book.ScheduleId)) {
        //        return Ok(new { result = "Error", detalle = "Already Booked this schedule", item = 0 });
        //    }

        //    book.TimeStamp = DateTime.Now;

        //    try {
        //        _context.Books.Add(book);
        //    } catch (Exception e)
        //    {
        //        return Ok(new { result = "Error", detalle = "Error", item = 0 });
        //    }

        //    _context.SaveChanges();
        //    return Ok(new { result = "Success", detalle = "Book success", idBook = _context.Books.LastOrDefault(a => a.Id > 0).Id });            
        //}

        public class BookList
        {            
            public List<Book> books { get; set; }            
        }

        [HttpPost]
        public IActionResult PostBook([FromBody] BookList books)
        {
            foreach (Book book in books.books)
            {
                if (_context.Books.Any(e => e.UserId == book.UserId && e.ScheduleId == book.ScheduleId)) {
                    return Ok(new { result = "Error", detalle = "Already Booked this schedule", item = 0 });
                }

                // VALIDACIÓN CUPO
                var schedule = _context.Schedules.FirstOrDefault(s => s.Id == book.ScheduleId);
                var maxCupo = _context.Activities.FirstOrDefault(a => a.Id == schedule.ActivityId).QuoteMax;                
                var currentBooks = _context.Books.Count(b => b.ScheduleId == book.ScheduleId);
                //if (currentBooks >= maxCupo) { return Ok(new { result = "Error", detalle = "No book space available", item = 0 }); }                

                book.TimeStamp = DateTime.Now;
                try {
                    _context.Books.Add(book);
                }
                catch (Exception e) {
                    return Ok(new { result = "Error", detalle = "Error", item = 0 });
                }
            }                                    

            _context.SaveChanges();
            return Ok(new { result = "Success", detalle = "Book success", idBook = _context.Books.LastOrDefault(a => a.Id > 0).Id });
        }

        // PUT: api/Books/5
        [HttpPut("{id}")]
        public IActionResult PutBook(int id, Book book)
        {           

            if (!BookExists(id)) { return NotFound(); }

            var bookDetail = _context.Books.FirstOrDefault(a => a.Id == id);
            bookDetail.Status = book.Status;

            try {
                _context.SaveChanges();
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = book });
            }
            catch (Exception e) {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            }
        }

        // PUT: api/Books/RemoveBook
        [Route("RemoveBook")]
        [HttpPut("{RemoveBook}")]        
        public IActionResult RemoveBook([FromBody] Book book)
        {
            var bookDetail = _context.Books.FirstOrDefault(b => b.ScheduleId == book.ScheduleId && b.UserId == book.UserId);
            if (bookDetail == null)
            {
                return NotFound();
            }

            try
            {
                _context.Remove(bookDetail);
                _context.SaveChanges();
                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = bookDetail });
            }
            catch (Exception e)
            {
                return Ok(new { result = "Error", detalle = "Error", item = 0 });
            }

        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return book;
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
